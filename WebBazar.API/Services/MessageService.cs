using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data;
using WebApp.API.DTOs.Message;
using WebApp.API.Extensions;
using WebApp.API.Helpers;
using WebApp.API.Models;
using WebApp.API.Services.Interfaces;

namespace WebApp.API.Services
{
    public class MessageService : BaseService, IMessageService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public MessageService(DataContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
            : base(context, mapper)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<MessageToReturnDTO>> CreateAsync(MessageForCreationDTO model)
        {
            var adExists = await _context
                .Ads
                .AnyAsync(a => a.Id == model.AdId);
                
            if (!adExists)
            {
                return "Обявата не е намерена";  
            }

            var recipientExists = await _context
                .Users
                .AnyAsync(u => u.Id == model.RecipientId);

            if (!recipientExists)
            {
                return "Получателят не е намерен";
            }

            var message = _mapper.Map<Message>(model);

            _context.Messages.Add(message);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<Message, MessageToReturnDTO>(await this.GetMessageByIdAsync(message.Id));
            }
            
            return "Грешка при запазване на съобщението";
        }

        private async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context
                .Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        
        public async Task<Result> DeleteAsync(int messageId, int currentUserId)
        {
            var message = await _context
                .Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
            {
                return "Съобщението не е намерено";
            }

            if (message.SenderId == currentUserId)
                message.SenderDeleted = true;

            if (message.RecipientId == currentUserId)
                message.RecipientDeleted = true;

            // if (message.SenderDeleted && message.RecipientDeleted)
            //     _context.Messages.Remove(message);
            
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return "Грешка при изтриване на съобщението";
        }

        public async Task<Result> MarkAsReadAsync(int messageId, int currentUserId)
        {
            var message = await _context
                .Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message.RecipientId != currentUserId)
            {
                return "Нямате право на тази операция";
            }
            
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<MessageToReturnDTO>> MessageThreadAsync(int adId, int currentUserId, int otherUserId)
        {
            var messages = await _context
                .Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .Where(m => (m.RecipientId == otherUserId && m.SenderId == currentUserId /*&& m.SenderDeleted == false*/
                          || m.RecipientId == currentUserId && m.SenderId == otherUserId /*&& m.RecipientDeleted == false*/)
                          && m.AdId == adId)
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MessageToReturnDTO>>(messages);
        }

        public async Task<int> UnreadMessagesCountAsync(int userId)
        {
            var unreadMsgsCount = await _context
                .Messages
                .Where(m => m.RecipientId == userId && m.IsRead == false && m.RecipientDeleted == false /* && m.SenderDeleted == false */)
                .CountAsync();
            
            return unreadMsgsCount;
        }

        public async Task<IEnumerable<MessageToReturnDTO>> UserMessagesAsync(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .AsQueryable();

            switch(messageParams.MessageContainer) 
            {
                case "Inbox":
                    messages = messages
                        .Where(m => m.RecipientId == messageParams.UserId /* && m.SenderDeleted == false */ && m.RecipientDeleted == false)
                        .OrderBy(m => m.IsRead)
                        .ThenByDescending(m => m.MessageSent);
                    break;
                case "Outbox":
                    messages = messages
                        .Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false)
                        .OrderByDescending(m => m.MessageSent);
                    break;
                default: // unread
                    messages = messages
                        .Where(m => m.RecipientId == messageParams.UserId && m.IsRead == false /* && m.SenderDeleted == false */ && m.RecipientDeleted == false)
                        .OrderByDescending(m => m.MessageSent);
                    break;
            }

            var paginatedMessages = await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
            
            _contextAccessor.HttpContext.Response.AddPagination(paginatedMessages.CurrentPage, paginatedMessages.PageSize,
                paginatedMessages.TotalCount, paginatedMessages.TotalPages);

            return _mapper.Map<IEnumerable<MessageToReturnDTO>>(paginatedMessages);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Message;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(DataContext data, IMapper mapper)
            : base(data, mapper) {}

        public async Task<PaginatedMessagesServiceModel> MineAsync(MessageParams messageParams, int userId)
        {
            var messages = GetFilteredMessages(messageParams.MessageFilter, userId);

            var paginatedMessages = await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

            return new PaginatedMessagesServiceModel
            {
                Messages = this.mapper.Map<IEnumerable<MessageToReturnDTO>>(paginatedMessages),
                CurrentPage = paginatedMessages.CurrentPage,
                PageSize = paginatedMessages.PageSize,
                TotalCount = paginatedMessages.TotalCount
            };
        }

        private IQueryable<Message> GetFilteredMessages(string messageFilter, int userId)
        {
            var messages = this.data.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .AsQueryable();

            switch (messageFilter)
            {
                case "Inbox":
                    messages = messages
                        .Where(m => m.RecipientId == userId /* && !(m.RecipientId == userId && m.IsRead == false && m.SenderDeleted == true) */)
                        .OrderBy(m => m.IsRead)
                        .ThenByDescending(m => m.SentOn);
                    break;
                case "Outbox":
                    messages = messages
                        .Where(m => m.SenderId == userId && m.SenderDeleted == false)
                        .OrderByDescending(m => m.SentOn);
                    break;
                default: // unread
                    messages = messages
                        .Where(m => m.RecipientId == userId && m.IsRead == false /* && m.SenderDeleted == false */)
                        .OrderByDescending(m => m.SentOn);
                    break;
            }

            return messages;
        }

        public async Task<IEnumerable<MessageToReturnDTO>> ThreadAsync(int adId, int senderId, int recipientId)
        {
            var messages = await this.data.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .Where(m => (m.RecipientId == recipientId && m.SenderId == senderId && m.SenderDeleted == false
                          || m.RecipientId == senderId && m.SenderId == recipientId /* && !(m.IsRead == false && m.SenderDeleted == true) */)
                          && m.AdId == adId
                          /* && !(m.IsRead == false && m.SenderDeleted == true) */)
                .OrderBy(m => m.SentOn)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<MessageToReturnDTO>>(messages);
        }

        public async Task<int> UnreadCountAsync(int userId)
        {
            return await this.data.Messages
                .Where(m => m.RecipientId == userId && m.IsRead == false /* && m.SenderDeleted == false */)
                .CountAsync();
        }

        public async Task<Result<DateTime>> CreateAsync(MessageForCreationDTO model)
        {
            var result = await ValidateInputModel(model);

            if (result.Failure)
            {
                return result.Error;
            }
            
            var message = this.mapper.Map<Message>(model);

            await this.data.AddAsync(message);
            await this.data.SaveChangesAsync();
            
            return message.SentOn;
        }

        private async Task<Result> ValidateInputModel(MessageForCreationDTO model)
        {
            var adExists = await this.data.Ads.AnyAsync(a => a.Id == model.AdId);
                
            if (!adExists)
            {
                return "Обявата не е намерена";  
            }

            var recipientExists = await this.data.Users.AnyAsync(u => u.Id == model.RecipientId);

            if (!recipientExists)
            {
                return "Получателят не е намерен";
            }

            return true;
        }
        
        public async Task<Result> MarkAsReadAsync(int id)
        {
            var message = await GetMessageAsync(id);

            if (message == null)
            {
                return "Съобщението не е намерено";
            }
            
            message.IsRead = true;
            message.ReadOn = DateTime.Now;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var message = await GetMessageAsync(id);

            if (message == null)
            {
                return "Съобщението не е намерено";
            }

            message.SenderDeleted = true;
            
            /*
            if (message.SenderId == userId)
            {
                message.SenderDeleted = true;
            }

            if (message.RecipientId == userId)
            {
                message.RecipientDeleted = true;
            }
            */

            await this.data.SaveChangesAsync();
            
            return true;
        }

        private async Task<Message> GetMessageAsync(int id)
        {
            return await this.data.Messages
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
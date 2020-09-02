using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class MessageRepository : RepositoryBase, IMessageRepository
    {
        public MessageRepository(DataContext context): base(context) { }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetUserMessages(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .AsQueryable();

            switch(messageParams.MessageContainer) 
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId);
                    break;
                default: // unread
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(u => u.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId, int adId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .Where(m => (m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false
                          || m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false) 
                          && m.AdId == adId)
                .ToListAsync();

            return messages;
        }

        public async Task<int> GetUnreadMessagesCount(int userId)
        {
            var unreadMsgsCount = await _context.Messages
                .Where(m => m.RecipientId == userId && m.IsRead == false && m.SenderDeleted == false && m.RecipientDeleted == false)
                .CountAsync();
            
            return unreadMsgsCount;
        }
    }
}
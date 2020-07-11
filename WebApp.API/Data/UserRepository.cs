using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<User> GetUser(int id, bool isCurrentUserOrAdminOrModerator)
        {
            var query = _context.Users.Include(u => u.Ads).AsQueryable();

            if (isCurrentUserOrAdminOrModerator) {
                query = query.IgnoreQueryFilters();
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public string GetPhotoUrl(int adId){
            return _context.Photos.Where(p => p.IsMain && p.AdId == adId).FirstOrDefault()?.Url;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        /*
        public Task<IEnumerable<Message>> GetMessagesForAd(int id)
        {
            throw new System.NotImplementedException();
        }
        */

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender)
                .Include(a => a.Ad)
                .Include(u => u.Recipient)
                .AsQueryable();
            /*
            var messages2 = _context.Ads
            .Include(a => a.Messages)
            .ThenInclude(m => m.Sender)
            .Include(a => a.Messages)
            .ThenInclude(m => m.Recipient)
            //.AsQueryable()
            .Where(a => a.UserId == messageParams.UserId);
            */
            switch(messageParams.MessageContainer) 
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId /*&& u.RecipientDeleted == false*/);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId /*&& u.SenderDeleted == false*/);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false /*&& u.RecipientDeleted == false*/ /*&& u.Ad.UserId == messageParams.UserId && u.IsRead == false*/);
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
                    || m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false) && m.AdId == adId)
                //.OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }

        public int GetUnreadMessagesCount(int userId)
        {
            return _context.Messages.Where(m => m.RecipientId == userId && m.IsRead == false && m.SenderDeleted == false && m.RecipientDeleted == false).Count();
        }
    }
}
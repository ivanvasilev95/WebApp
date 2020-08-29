using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IMessageRepository : IBaseRepository
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetUserMessages(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId, int adId);
        Task<int> GetUnreadMessagesCount(int userId); 
    }
}
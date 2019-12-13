using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public interface IUserRepository
    {
         Task<User> GetUser(int id);
         Task<bool> SaveAll();
         string getPhotoUrl(int id);
        Task<Message> GetMessage(int id);
         //Task<IEnumerable<Message>> GetMessagesForAd(MessageParams messageParams); // int id // Task<PagedList<Message>>
         IEnumerable<Message> GetMessagesForUser(MessageParams messageParams); // Task<PagedList<Message>>
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId, int adId); 
    }
}
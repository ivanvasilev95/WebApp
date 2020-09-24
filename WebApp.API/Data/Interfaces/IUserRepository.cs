using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id, bool includeNotApprovedUserAds);
        Task<bool> EmailIsNotAvailable(int userId, string email);
        Task<bool> SaveAll();
    }
}
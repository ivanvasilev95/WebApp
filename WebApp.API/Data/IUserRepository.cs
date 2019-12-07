using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public interface IUserRepository
    {
         Task<User> GetUser(int id);
         Task<bool> SaveAll();

         string getPhotoUrl(int id);
    }
}
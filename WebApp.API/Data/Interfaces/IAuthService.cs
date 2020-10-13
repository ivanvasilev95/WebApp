using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
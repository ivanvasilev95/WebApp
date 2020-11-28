using System.Threading.Tasks;
using WebApp.API.Data.Models;

namespace WebApp.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
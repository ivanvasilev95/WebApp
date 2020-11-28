using System.Threading.Tasks;
using WebBazar.API.Data.Models;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
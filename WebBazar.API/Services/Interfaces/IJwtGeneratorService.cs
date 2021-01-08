using System.Threading.Tasks;
using WebBazar.API.Data.Models;

namespace WebBazar.API.Services.Interfaces
{
    public interface IJwtGeneratorService
    {
        Task<string> GenerateJwtTokenAsync(User user);
    }
}
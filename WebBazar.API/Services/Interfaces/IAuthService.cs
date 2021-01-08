using System.Threading.Tasks;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginServiceModel>> LoginAsync(UserForLoginDTO model);
        Task<Result> RegisterAsync(UserForRegisterDTO model);
    }
}
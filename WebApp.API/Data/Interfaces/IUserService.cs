using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.API.DTOs.User;
using WebApp.API.Helpers;

namespace WebApp.API.Data.Interfaces
{
    public interface IUserService
    {
         Task<Result<UserForDetailedDTO>> GetUserWithAdsAsync(int id, ClaimsPrincipal currentUser);
         Task<UserForUpdateDTO> GetUserForEditAsync(int id);
         Task<Result> UpdateUserAsync(int id, UserForUpdateDTO model);
    }
}
using System.Threading.Tasks;
using WebApp.API.DTOs.User;
using WebApp.API.Helpers;

namespace WebApp.API.Services.Interfaces
{
    public interface IUserService
    {
         Task<Result<UserForDetailedDTO>> GetUserWithAdsAsync(int id, bool includeNotApprovedAds);
         Task<UserForUpdateDTO> GetUserForEditAsync(int id);
         Task<Result> UpdateUserAsync(int id, UserForUpdateDTO model);
    }
}
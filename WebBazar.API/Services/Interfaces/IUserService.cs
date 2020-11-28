using System.Threading.Tasks;
using WebBazar.API.DTOs.User;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IUserService
    {
         Task<Result<UserForDetailedDTO>> GetUserWithAdsAsync(int id, bool includeNotApprovedAds);
         Task<UserForUpdateDTO> GetUserForEditAsync(int id);
         Task<Result> UpdateUserAsync(int id, UserForUpdateDTO model);
    }
}
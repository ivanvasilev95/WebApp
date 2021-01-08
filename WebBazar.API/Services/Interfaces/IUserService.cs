using System.Threading.Tasks;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IUserService
    {
         Task<UserForDetailedDTO> GetWithAdsAsync(int id, bool includeNotApprovedAds);
         Task<UserForUpdateDTO> DetailsAsync(int id);
         Task<Result> UpdateAsync(int id, UserForUpdateDTO model);
    }
}
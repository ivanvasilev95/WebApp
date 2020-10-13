using System.Threading.Tasks;
using WebApp.API.DTOs.User;
using WebApp.API.Helpers;

namespace WebApp.API.Data.Interfaces
{
    public interface IUserService
    {
         Task<Result<UserForDetailedDTO>> ByIdAsync(int id, bool includeNotApprovedAds);
         Task<Result> UpdateAsync(int id, UserForUpdateDTO model);
    }
}
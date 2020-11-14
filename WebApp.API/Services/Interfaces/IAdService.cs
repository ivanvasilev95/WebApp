using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApp.API.DTOs.Ad;
using WebApp.API.Helpers;

namespace WebApp.API.Services.Interfaces
{
    public interface IAdService
    {
         Task<Result<AdForDetailedDTO>> ByIdAsync(int id);
         Task<IEnumerable<AdForListDTO>> AllAsync(UserParams userParams);
         Task<IEnumerable<AdForListDTO>> UserAdsAsync(int userId);
         Task<IEnumerable<AdForListDTO>> UserLikedAdsAsync(int userId);
         Task<int> CreateAsync(AdForCreateDTO model);

        Task<Result> UpdateAsync(int id, AdForUpdateDTO model);

        Task<Result> DeleteAsync(int id);


    }
}
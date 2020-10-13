using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApp.API.DTOs.Ad;
using WebApp.API.Helpers;

namespace WebApp.API.Data.Interfaces
{
    public interface IAdService
    {
         Task<Result<AdForDetailedDTO>> ByIdAsync(int id);
         Task<IEnumerable<AdForListDTO>> AllAsync(UserParams userParams, HttpResponse response);
         Task<IEnumerable<AdForListDTO>> UserAdsAsync(int userId);
         Task<IEnumerable<AdForListDTO>> UserLikedAdsAsync(int userId);
         Task<int> CreateAsync(AdForCreateDTO model);

        Task<Result> UpdateAsync(int id, AdForUpdateDTO adForUpdateDTO);

        Task<Result> DeleteAsync(int id);


    }
}
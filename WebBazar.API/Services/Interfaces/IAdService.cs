using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAdService
    {
         Task<Result<AdForDetailedDTO>> ByIdAsync(int id);
         Task<IEnumerable<AdForListDTO>> AllAsync(AdParams adParams);
         Task<IEnumerable<AdForListDTO>> UserAdsAsync(int userId);
         Task<IEnumerable<AdForListDTO>> UserLikedAdsAsync(int userId);
         Task<int> CreateAsync(AdForCreateDTO model);

        Task<Result> UpdateAsync(int id, AdForUpdateDTO model);

        Task<Result> DeleteAsync(int id);


    }
}
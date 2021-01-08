using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IAdService
    {
        Task<PaginatedAdsServiceModel> AllAsync(AdParams adParams);
        Task<IEnumerable<AdForListDTO>> MineAsync(int userId);
        Task<IEnumerable<AdForListDTO>> LikedAsync(int userId);
        Task<AdForDetailedDTO> DetailsAsync(int id);
        Task<int> CreateAsync(AdForCreateDTO model);
        Task<Result> UpdateAsync(int id, AdForUpdateDTO model);
        Task<Result> DeleteAsync(int id);
    }
}
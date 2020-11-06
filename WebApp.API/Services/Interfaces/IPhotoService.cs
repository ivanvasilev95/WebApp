using System.Threading.Tasks;
using WebApp.API.DTOs.Photo;
using WebApp.API.Helpers;

namespace WebApp.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<PhotoForReturnDTO>> AddAsync(int adId, PhotoForCreationDTO model);
        Task<Result> DeleteAsync(int id);
        Task<Result> SetMainAsync(int id);
    }
}
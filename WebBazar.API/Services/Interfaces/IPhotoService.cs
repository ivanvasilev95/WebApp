using System.Threading.Tasks;
using WebBazar.API.DTOs.Photo;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<PhotoForReturnDTO>> AddAsync(int adId, PhotoForCreationDTO model);
        Task<Result> DeleteAsync(int id);
        Task<Result> SetMainAsync(int id);
    }
}
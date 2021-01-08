using System.Threading.Tasks;
using WebBazar.API.DTOs.Photo;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<PhotoForDetailedDTO>> AddAsync(PhotoForCreationDTO model, int adId);
        Task<Result> SetMainAsync(int id);
        Task<Result> DeleteAsync(int id);
    }
}
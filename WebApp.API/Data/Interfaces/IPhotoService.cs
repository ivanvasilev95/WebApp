using System.Threading.Tasks;
using WebApp.API.DTOs.Photo;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IPhotoService
    {
        Task<Result<Photo>> CreateAsync(int adId, PhotoForCreationDTO model);
        Task<Result> DeleteAsync(int id);
        Task<Result> SetMainAsync(int id);
        Task<Result<PhotoForReturnDTO>> ByIdAsync(int id);
    }
}
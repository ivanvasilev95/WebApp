using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface IPhotoRepository : IRepositoryBase
    {
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetAdMainPhoto(int adId);
    }
}
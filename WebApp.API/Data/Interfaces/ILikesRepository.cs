using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface ILikesRepository : IBaseRepository<Like>
    {
        Task<Like> GetLike(int userId, int adId);
        Task<int> GetAdLikesCount(int adId);
    }
}
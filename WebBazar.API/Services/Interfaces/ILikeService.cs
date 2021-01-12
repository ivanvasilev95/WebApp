using System.Threading.Tasks;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface ILikeService
    {
         Task<Result> LikeAdAsync(int adId, int userId);
         Task<Result> UnlikeAdAsync(int adId, int userId);
    }
}
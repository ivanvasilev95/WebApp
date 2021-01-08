using System.Threading.Tasks;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface ILikeService
    {
         Task<Result> LikeAsync(int adId, int userId);
         Task<Result> UnlikeAsync(int adId, int userId);
    }
}
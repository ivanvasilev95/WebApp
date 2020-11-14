using System.Threading.Tasks;
using WebApp.API.Helpers;

namespace WebApp.API.Services.Interfaces
{
    public interface ILikeService
    {
         Task<Result> Like(int userId, int adId);
         Task<Result> Unlike(int userId, int adId);
         Task<int> AdLikesCount(int adId);
    }
}
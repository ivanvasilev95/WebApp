using System.Threading.Tasks;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface ILikeService
    {
         Task<Result> Like(int userId, int adId);
         Task<Result> Unlike(int userId, int adId);
    }
}
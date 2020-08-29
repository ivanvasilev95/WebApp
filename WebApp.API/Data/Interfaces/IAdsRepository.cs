using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp.API.Models;
using WebApp.API.Helpers;

namespace WebApp.API.Data.Interfaces
{
    public interface IAdsRepository : IBaseRepository
    {
        Task<PagedList<Ad>> GetAds(UserParams userParams);
        Task<Ad> GetAd(int id);
        Task<IEnumerable<Ad>> GetUserAds(int userId);
        Task<IEnumerable<Ad>> GetUserLikedAds(int userId);
    }
}
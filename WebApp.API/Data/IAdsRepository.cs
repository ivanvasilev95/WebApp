using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public interface IAdsRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<IEnumerable<Ad>> GetAds();
         Task<Ad> GetAd(int id);
         IEnumerable<Ad> GetUserAds(int userId);
         Category GetAdCategoryName(int categoryId);
         IEnumerable<Category> GetCategories();
         Task<Like> GetLike(int userId, int adId);
         int GetAdLikesCount(int adId);
         IEnumerable<Ad> GetUserFavorites(int userId);
    }
}
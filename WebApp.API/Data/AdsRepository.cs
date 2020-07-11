using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public class AdsRepository : IAdsRepository
    {
        private readonly DataContext _context;
        public AdsRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Ad> GetAd(int id)
        {
            var ad = await _context.Ads.Include(a => a.Photos).IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            return ad;
        }
        /*
        public async Task<PagedList<Ad>> GetAds(UserParams userParams)
        {
            var ads = _context.Ads.Include(a => a.Photos);

            return await PagedList<Ad>.CreateAsync(ads, userParams.PageNumber, userParams.PageSize);
        }
        */

        public async Task<PagedList<Ad>> GetAds(UserParams userParams)
        {
            var ads = _context.Ads.Include(a => a.Photos).AsQueryable();
            
            if (!string.IsNullOrEmpty(userParams.SearchText)) {
                ads = ads.Where(ad => ad.Title.ToLower().Contains(userParams.SearchText) || ad.Location.ToLower().Contains(userParams.SearchText));
            }
            
            if (userParams.CategoryId != 0) {
                ads = ads.Where(ad => ad.CategoryId == userParams.CategoryId);
            }
            
            switch (userParams.SortCriteria) {
                case "negotiation": { // po dogovarqne
                    ads = ads.Where(a => a.Price == null);
                    break;
                }
                case "cheapest": {
                    ads = ads.Where(a => a.Price != null).OrderBy(a => a.Price);
                    break;
                }
                case "expensive": {
                    ads = ads.Where(a => a.Price != null).OrderByDescending(a => a.Price);
                    break;
                }
                default: { // newest
                    ads = ads.OrderByDescending(a => a.DateAdded);
                    break;
                }
            }

            return await PagedList<Ad>.CreateAsync(ads, userParams.PageNumber, userParams.PageSize);
        }

        public IEnumerable<Ad> GetUserAds(int userId)
        {
            var ads =  _context.Ads.Include(a => a.Photos).IgnoreQueryFilters();

            return ads.Where(a => a.UserId == userId).OrderByDescending(a => a.DateAdded).ToList();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Category GetAdCategory(int categoryId) {
            return  _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public IEnumerable<Category> GetCategories() {
            return _context.Categories.ToList();
            //return _context.Categories.Include(c => c.Ads).ToList();
        }

        public async Task<Like> GetLike(int userId, int adId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.UserId == userId && u.AdId == adId);
        }

        public int GetAdLikesCount(int adId)
        {
            return _context.Likes.Where(l => l.AdId == adId).Count();
        }

        public IEnumerable<Ad> GetUserFavorites(int userId)
        {
            var user = _context.Users.Include(x => x.Likes).FirstOrDefault(u => u.Id == userId);
            //var userFavorites = _context.Likes.Where(u => u.UserId == userId).Select(i => i.AdId);
            var userFavorites = user.Likes.Select(i => i.AdId);
            
            return _context.Ads.Include(p => p.Photos).Where(a => userFavorites.Contains(a.Id));  
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Photo> GetMainPhotoForAd(int adId)
        {
            return await _context.Photos.Where(p => p.AdId == adId).FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}
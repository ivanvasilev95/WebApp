using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class AdsRepository : RepositoryBase, IAdsRepository
    {
        public AdsRepository(DataContext context) : base(context) { }

        public async Task<Ad> GetAd(int id)
        {
            var ad = await _context.Ads
                .Include(a => a.Category)
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

            return ad;
        }

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

        public async Task<IEnumerable<Ad>> GetUserAds(int userId)
        {
            var ads =  await _context.Ads
                .Include(a => a.Photos)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.IsApproved)
                .ThenByDescending(a => a.DateAdded)
                .IgnoreQueryFilters()
                .ToListAsync();

            return ads;
        }

        public async Task<IEnumerable<Ad>> GetUserLikedAds(int userId)
        {
            var user = await _context.Users.Include(x => x.Likes).FirstOrDefaultAsync(u => u.Id == userId);
            var userLikedAds = user.Likes.Select(i => i.AdId);
            
            return _context.Ads.Include(p => p.Photos).Where(a => userLikedAds.Contains(a.Id));  
        }
    }
}
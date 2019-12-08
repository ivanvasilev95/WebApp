using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var ad = await _context.Ads.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id);

            return ad;
        }

        public async Task<IEnumerable<Ad>> GetAds()
        {
            var ads = await _context.Ads.Include(a => a.Photos).ToListAsync();

            return ads;
        }

        public IEnumerable<Ad> GetUserAds(int userId)
        {
            var ads =  _context.Ads.Include(a => a.Photos).Where(a => a.UserId == userId).OrderByDescending(a => a.DateAdded).ToList();
            
            return ads;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Category GetAdCategoryName(int categoryId) {
            return  _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public IEnumerable<Category> GetCategories() {
            return _context.Categories.ToList();
            //return _context.Categories.Include(c => c.Ads).ToList();
        }
    }
}
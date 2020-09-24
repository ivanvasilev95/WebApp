using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int id, bool includeNotApprovedUserAds)
        {
            var query = _context.Users.Include(u => u.Ads).AsQueryable();

            if (includeNotApprovedUserAds) {
                query = query.IgnoreQueryFilters();
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> EmailIsNotAvailable(int userId, string email)
        {
            return await _context.Users.AnyAsync(u => u.Id != userId && u.Email == email);
        }
    }
}
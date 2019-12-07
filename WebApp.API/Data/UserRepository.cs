using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Ads).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public string getPhotoUrl(int adId){
            return _context.Photos.Where(p => p.IsMain && p.AdId == adId).FirstOrDefault()?.Url;
        }
    }
}
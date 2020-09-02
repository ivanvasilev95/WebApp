using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class LikesRepository : RepositoryBase, ILikesRepository
    {
        public LikesRepository(DataContext context) : base(context){ }

        public async Task<Like> GetLike(int userId, int adId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.UserId == userId && u.AdId == adId);
        }

        public async Task<int> GetAdLikesCount(int adId)
        {
            return await _context.Likes.Where(l => l.AdId == adId).CountAsync();
        }
    }
}
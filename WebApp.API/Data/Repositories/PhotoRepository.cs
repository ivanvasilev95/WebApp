using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context){ }

        public async Task<Photo> GetAdMainPhoto(int adId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.AdId == adId && p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
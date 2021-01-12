using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.Data.Models;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class LikeService : BaseService, ILikeService
    {
        public LikeService(DataContext data, IMapper mapper)
            : base(data, mapper) {}

        public async Task<Result> LikeAdAsync(int adId, int userId)
        {
            var adAlreadyLiked = await this.data.Likes
                .AnyAsync(l => l.UserId == userId && l.AdId == adId);

            if (adAlreadyLiked)
            {
                return "Обявата вече е добавена в Наблюдавани";
            }

            var like = new Like
            {
                UserId = userId,
                AdId = adId
            };

            await this.data.AddAsync(like);
            await this.data.SaveChangesAsync();
            
            return true;
        }

        public async Task<Result> UnlikeAdAsync(int adId, int userId)
        {
            var like = await this.data.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.AdId == adId);

            if (like == null)
            {
                return "Лайкът не е намерен";
            }

            this.data.Remove(like);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
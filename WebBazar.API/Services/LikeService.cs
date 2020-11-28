using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data;
using WebApp.API.Helpers;
using WebApp.API.Data.Models;
using WebApp.API.Services.Interfaces;

namespace WebApp.API.Services
{
    public class LikeService : BaseService, ILikeService
    {
        public LikeService(DataContext context, IMapper mapper)
            : base(context, mapper) {}
        
        public async Task<int> AdLikesCount(int adId)
        {
            return await _context
                .Likes
                .Where(l => l.AdId == adId)
                .CountAsync();
        }

        public async Task<Result> Like(int userId, int adId)
        {
            var adAlreadyLiked = await _context
                .Likes
                .AnyAsync(l => l.UserId == userId && l.AdId == adId);

            if (adAlreadyLiked)
            {
                return "Обявата вече е добавена в Наблюдавани";
            }

            var ad = await _context
                .Ads
                .Where(a => a.Id == adId)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
               return "Обявата не е намерена"; 
            }

            if (ad.UserId == userId)
            {
                return "Не може да добавяте собствени обяви в Наблюдавани";
            }

            _context.Likes.Add(new Like
            {
                UserId = userId,
                AdId = adId
            });

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            
            return "Грешка при добавяне на обявата в Наблюдавани";
        }

        public async Task<Result> Unlike(int userId, int adId)
        {
            var like = await _context
                .Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.AdId == adId);

            if(like == null)
                return "Лайкът не е намерен";

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
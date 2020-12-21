using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Helpers;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class AdService : BaseService, IAdService
    {
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public AdService(
            DataContext context,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig) : base(context, mapper)
        {
            _cloudinaryConfig = cloudinaryConfig;
        }

        public async Task<PaginatedAdsServiceModel> AllAsync(AdParams adParams)
        {
            var ads = _context.Ads
                .Include(a => a.Photos)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(adParams.SearchText))
            {
                ads = ads
                    .Where(ad => ad.Title.ToLower()
                    .Contains(adParams.SearchText) || ad.Location.ToLower().Contains(adParams.SearchText));
            }
            
            if (adParams.CategoryId != 0)
            {
                ads = ads
                    .Where(ad => ad.CategoryId == adParams.CategoryId);
            }
            
            switch (adParams.SortCriteria)
            {
                case "negotiation": // po dogovarqne
                    ads = ads
                        .Where(a => a.Price == null);
                    break;
                case "cheapest":
                    ads = ads
                        .Where(a => a.Price != null)
                        .OrderBy(a => a.Price);
                    break;
                case "expensive":
                    ads = ads
                        .Where(a => a.Price != null)
                        .OrderByDescending(a => a.Price);
                    break;
                default: // newest
                    ads = ads
                        .OrderByDescending(a => a.DateAdded);
                    break;
            }

            var paginatedAds = await PagedList<Ad>.CreateAsync(ads, adParams.PageNumber, adParams.PageSize);

            return new PaginatedAdsServiceModel
            {
                Ads = _mapper.Map<IEnumerable<AdForListDTO>>(paginatedAds),
                CurrentPage = paginatedAds.CurrentPage,
                PageSize = paginatedAds.PageSize,
                TotalCount = paginatedAds.TotalCount
            };
        }

        public async Task<Result<AdForDetailedDTO>> ByIdAsync(int id)
        {
            var ad = await _context.Ads
                .Include(a => a.Category)
                .Include(a => a.Photos)
                .Include(a => a.User)
                .Include(a => a.Likes)
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

            return _mapper.Map<Ad, AdForDetailedDTO>(ad);
        }

        public async Task<int> CreateAsync(AdForCreateDTO model)
        {
            var ad = _mapper.Map<Ad>(model);

            _context.Add(ad);

            await _context.SaveChangesAsync();

            return ad.Id;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var ad = await _context.Ads
                // .Include(a => a.Photos)
                // .Include(a => a.Likes)
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

            /*
            if (ad.Likes.Any())
            {
                RemoveAdLikes(ad);
            }

            if (ad.Photos.Any(p => !p.IsDeleted))
            {
                RemoveAdPhotos(ad);
            }
            */

            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();

            return true;
        }

        /*
        private void RemoveAdPhotos(Ad ad) 
        {
            var acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            var cloudinary = new Cloudinary(acc);

            foreach (var photo in ad.Photos.Where(p => !p.IsDeleted)) 
            {
                if (photo.PublicId != null) 
                {
                    var deleteParams = new DeletionParams(photo.PublicId);
					cloudinary.Destroy(deleteParams);
                }

                _context.Photos.Remove(photo);
            }
        }

        private void RemoveAdLikes(Ad ad)
        {
            foreach (var like in ad.Likes)
            {
                _context.Likes.Remove(like);
            }
        }
        */

        public async Task<Result> UpdateAsync(int id, AdForUpdateDTO model)
        {
            var ad = await _context.Ads
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

             _mapper.Map(model, ad);

             return true;
        }

        public async Task<IEnumerable<AdForListDTO>> MineAsync(int userId)
        {
            var ads =  await _context.Ads
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .Where(a => a.UserId == userId && a.IsDeleted == false)
                .OrderBy(a => a.IsApproved)
                .ThenByDescending(a => a.DateAdded)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AdForListDTO>>(ads);
        }

        public async Task<IEnumerable<AdForListDTO>> LikedAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var userLikedAdsIds = user.Likes
                .Select(i => i.AdId);

            var userLikedAds = _context.Ads
                .Include(p => p.Photos)
                .Where(a => userLikedAdsIds.Contains(a.Id));
            
            return _mapper.Map<IEnumerable<AdForListDTO>>(userLikedAds);
        }
    }
}
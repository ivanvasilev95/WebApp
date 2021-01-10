using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
// using CloudinaryDotNet;
// using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Options;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services
{
    public class AdService : BaseService, IAdService
    {
        // private IOptions<CloudinarySettings> cloudinaryConfig;

        public AdService(
            DataContext data,
            IMapper mapper
            /* IOptions<CloudinarySettings> cloudinaryConfig */) : base(data, mapper)
        {
            // this.cloudinaryConfig = cloudinaryConfig;
        }

        public async Task<PaginatedAdsServiceModel> AllAsync(AdParams adParams)
        {
            var ads = GetFilteredAds(adParams.SearchText, adParams.CategoryId, adParams.SortCriteria);

            var paginatedAds = await PagedList<Ad>.CreateAsync(ads, adParams.PageNumber, adParams.PageSize);

            return new PaginatedAdsServiceModel
            {
                Ads = this.mapper.Map<IEnumerable<AdForListDTO>>(paginatedAds),
                CurrentPage = paginatedAds.CurrentPage,
                PageSize = paginatedAds.PageSize,
                TotalCount = paginatedAds.TotalCount
            };
        }

        private IQueryable<Ad> GetFilteredAds(string searchText, int categoryId, string sortCriteria)
        {
            var ads = this.data.Ads
                .Include(a => a.Photos)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(searchText))
            {
                ads = ads
                    .Where(ad => ad.Title.ToLower()
                    .Contains(searchText) || ad.Location.ToLower().Contains(searchText));
            }
            
            if (categoryId != 0)
            {
                ads = ads
                    .Where(ad => ad.CategoryId == categoryId);
            }
            
            switch (sortCriteria)
            {
                case "negotiation": // po dogovarqne
                    ads = ads.Where(a => a.Price == null);
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
                    ads = ads.OrderByDescending(a => a.DateAdded);
                    break;
            }

            return ads;
        }

        public async Task<IEnumerable<AdForListDTO>> MineAsync(int userId)
        {
            var ads =  await this.data.Ads
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .Where(a => a.UserId == userId && a.IsDeleted == false)
                .OrderBy(a => a.IsApproved)
                .ThenByDescending(a => a.DateAdded)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<AdForListDTO>>(ads);
        }

        public async Task<IEnumerable<AdForListDTO>> LikedAsync(int userId)
        {
            var likedAds = await this.data.Likes
                .Include(l => l.Ad)
                .ThenInclude(a => a.Photos)
                .Where(l => l.UserId == userId)
                .Select(l => l.Ad)
                .ToListAsync();
            
            return this.mapper.Map<IEnumerable<AdForListDTO>>(likedAds);
        }

        public async Task<AdForDetailedDTO> DetailsAsync(int id)
        {
            var ad = await this.data.Ads
                .Include(a => a.Category)
                .Include(a => a.Photos)
                .Include(a => a.User)
                .Include(a => a.Likes)
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            return this.mapper.Map<Ad, AdForDetailedDTO>(ad);
        }

        public async Task<int> CreateAsync(AdForCreateDTO model)
        {
            var ad = this.mapper.Map<Ad>(model);

            this.data.Add(ad);
            await this.data.SaveChangesAsync();

            return ad.Id;
        }

        public async Task<Result> UpdateAsync(int id, AdForUpdateDTO model)
        {
            var ad = await this.data.Ads
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

             this.mapper.Map(model, ad);

             return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var ad = await this.data.Ads
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

            this.data.Remove(ad);
            await this.data.SaveChangesAsync();

            return true;
        }

        /*
        private void RemoveAdPhotos(Ad ad) 
        {
            var acc = new Account(
                this.cloudinaryConfig.Value.CloudName,
                this.cloudinaryConfig.Value.ApiKey,
                this.cloudinaryConfig.Value.ApiSecret);

            var cloudinary = new Cloudinary(acc);

            foreach (var photo in ad.Photos.Where(p => !p.IsDeleted)) 
            {
                if (photo.PublicId != null) 
                {
                    var deleteParams = new DeletionParams(photo.PublicId);
					cloudinary.Destroy(deleteParams);
                }

                this.data.Photos.Remove(photo);
            }
        }

        private void RemoveAdLikes(Ad ad)
        {
            foreach (var like in ad.Likes)
            {
                this.data.Likes.Remove(like);
            }
        }
        */
    }
}
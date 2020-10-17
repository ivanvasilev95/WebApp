using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Ad;
using WebApp.API.Extensions;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Services
{
    public class AdService : IAdService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AdService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AdForListDTO>> AllAsync(UserParams userParams, HttpResponse response)
        {
            var ads = _context
                .Ads
                .Include(a => a.Photos)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(userParams.SearchText))
            {
                ads = ads
                    .Where(ad => ad.Title.ToLower()
                    .Contains(userParams.SearchText) || ad.Location.ToLower().Contains(userParams.SearchText));
            }
            
            if (userParams.CategoryId != 0)
            {
                ads = ads
                    .Where(ad => ad.CategoryId == userParams.CategoryId);
            }
            
            switch (userParams.SortCriteria)
            {
                case "negotiation": // po dogovarqne
                { 
                    ads = ads
                        .Where(a => a.Price == null);
                    break;
                }
                case "cheapest":
                {
                    ads = ads
                        .Where(a => a.Price != null)
                        .OrderBy(a => a.Price);
                    break;
                }
                case "expensive":
                {
                    ads = ads
                        .Where(a => a.Price != null)
                        .OrderByDescending(a => a.Price);
                    break;
                }
                default: // newest
                { 
                    ads = ads
                        .OrderByDescending(a => a.DateAdded);
                    break;
                }
            }

            var paginatedAds = await PagedList<Ad>.CreateAsync(ads, userParams.PageNumber, userParams.PageSize);

            response.AddPagination(paginatedAds.CurrentPage, paginatedAds.PageSize, paginatedAds.TotalCount, paginatedAds.TotalPages);

            return _mapper.Map<IEnumerable<AdForListDTO>>(paginatedAds);
        }

        public async Task<Result<AdForDetailedDTO>> ByIdAsync(int id)
        {
            var ad = await _context.Ads
                .Include(a => a.Category)
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id);

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
            var ad = await this.GetAdAsync(id);

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Result> UpdateAsync(int id, AdForUpdateDTO adForUpdateDTO)
        {
            var ad = await GetAdAsync(id);
            if (ad == null)
            {
                return "Обявата не е намерена";
            }

             _mapper.Map(adForUpdateDTO, ad);

             return true;
        }

        private async Task<Ad> GetAdAsync(int id)
        {
            var ad = await _context
                .Ads
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            return ad;
        }

        public async Task<IEnumerable<AdForListDTO>> UserAdsAsync(int userId)
        {
            var ads =  await _context.Ads
                .Include(a => a.Photos)
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.IsApproved)
                .ThenByDescending(a => a.DateAdded)
                .IgnoreQueryFilters()
                .ToListAsync();

            return _mapper.Map<IEnumerable<AdForListDTO>>(ads);
        }

        public async Task<IEnumerable<AdForListDTO>> UserLikedAdsAsync(int userId)
        {
            var user = await _context
                .Users
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var userLikedAdsIds = user
                .Likes
                .Select(i => i.AdId);

            var userLikedAds = _context
                .Ads
                .Include(p => p.Photos)
                .Where(a => userLikedAdsIds.Contains(a.Id));
            
            return _mapper.Map<IEnumerable<AdForListDTO>>(userLikedAds);
        }
    }
}
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebBazar.API.Data.Models;
using AutoMapper;
using WebBazar.API.DTOs.Ad;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using WebBazar.API.Services.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using WebBazar.API.Helpers;
using WebBazar.API.Data;

namespace WebBazar.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        public AdminService(
            DataContext context,
            UserManager<User> userManager,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _context = context;
            _userManager = userManager;
            _cloudinaryConfig = cloudinaryConfig;
        }

        public async Task<dynamic> GetUsersWithRolesAsync()
        {
            var userList = await (from user in _context.Users
                                  orderby user.Id
                                  select new
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in _context.Roles
                                               on userRole.RoleId
                                               equals role.Id
                                               orderby role.Name
                                               select role.Name).ToList()
                                  }).ToListAsync();

            return userList;
        }

        public async Task<string[]> GetRolesAsync()
        {
            var roles = await _context.Roles
                .OrderBy(r => r.Name)
                .Select(r => r.Name)
                .ToArrayAsync();

            return roles;
        }

        public async Task<List<AdForListDTO>> GetAdsForApprovalAsync()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Ad, AdForListDTO>().ForMember(
                    dto => dto.PhotoUrl, 
                    conf => conf.MapFrom(ol => ol.Photos.FirstOrDefault(p => p.IsMain && !p.IsDeleted).Url))
            );

            var ads = await _context.Ads
                // .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .Where(a => a.IsApproved == false && a.IsDeleted == false)
                .OrderByDescending(a => a.DateAdded)
                .ProjectTo<AdForListDTO>(configuration)
                .ToListAsync();

            return ads;
        }

        public async Task<Result> EditUserRolesAsync(string userName, string[] selectedRoles)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);

            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                return "Грешка при добавянето на роли.";
            }
            
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                return "Грешка при премахването на роли.";
            }

            return true;
        }

        public async Task<Result> ApproveAdAsync(int id) 
        {
            var ad = await _context.Ads
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

            ad.IsApproved = true;

            await _context.SaveChangesAsync();

            return true;
        }

        /*
        public async Task RejectAd(int id) 
        {
            var ad = await _context.Ads
                // .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();
			
            // ad.Photos = ad.Photos.Where(a => a.IsDeleted == false).ToList();
            
			// if (ad.Photos.Any())
            // {
                // RemoveAdPhotos(ad);
            // }

            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();
        }

        private void RemoveAdPhotos(Ad ad) 
        {
            var acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            var cloudinary = new Cloudinary(acc);

            foreach (var photo in ad.Photos) 
            {
                if (photo.PublicId != null) 
                {
                    var deleteParams = new DeletionParams(photo.PublicId);
					cloudinary.Destroy(deleteParams);
                }

                _context.Photos.Remove(photo);
            }
        }
        */
    }
}
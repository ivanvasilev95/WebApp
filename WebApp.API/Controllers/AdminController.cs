using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApp.API.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using WebApp.API.Helpers;
using CloudinaryDotNet;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Actions;
using WebApp.API.DTOs.Ad;
using WebApp.API.DTOs.Role;

namespace WebApp.API.Controllers
{
    public class AdminController : ApiController
    {
        private readonly DataContext _context;
        public readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public AdminController(
            DataContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
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

            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => r.Name)
                .ToArrayAsync();

            return Ok(roles);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = roleEditDTO.RoleNames;

            selectedRoles = selectedRoles ?? new string[] {};
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if(!result.Succeeded)
                return BadRequest("Грешка при добавянето на роли.");
            
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded)
                return BadRequest("Грешка при премахването на роли.");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpGet("adsForModeration")]
        public async Task<IActionResult> GetAdsForModeration()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Ad, AdForListDTO>().ForMember(
                    dto => dto.PhotoUrl, 
                    conf => conf.MapFrom(ol => ol.Photos.FirstOrDefault(p => p.IsMain).Url))
            );

            var ads = await _context.Ads
                .IgnoreQueryFilters()
                .Where(a => a.IsApproved == false)
                .ProjectTo<AdForListDTO>(configuration)
                .ToListAsync();

            return Ok(ads);
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpPost("approveAd/{adId}")]
        public async Task<IActionResult> ApproveAd(int adId) 
        {
            var ad = await _context.Ads
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == adId);

            ad.IsApproved = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Policy = "ModerateAdRole")]
        [HttpPost("rejectAd/{adId}")]
        public async Task<IActionResult> RejectAd(int adId) 
        {
            var ad = await _context.Ads
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == adId);
            
            RemoveAdPhotos(ad);

            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private void RemoveAdPhotos(Ad ad) {
            foreach (var photo in ad.Photos) 
            {
                if (photo.PublicId != null) 
                {
                    var deleteParams = new DeletionParams(photo.PublicId);
					_cloudinary.Destroy(deleteParams);
                }

                _context.Photos.Remove(photo);
            }
        }
    }
}
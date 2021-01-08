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
using WebBazar.API.Data;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly DataContext data;
        private readonly UserManager<User> userManager;

        public AdminService(DataContext data, UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserWithRolesServiceModel>> UsersWithRolesAsync()
        {
            var users = await (from user in this.data.Users
                                  orderby user.Id
                                  select new UserWithRolesServiceModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in this.data.Roles
                                               on userRole.RoleId
                                               equals role.Id
                                               orderby role.Name
                                               select role.Name).ToList()
                                  }).ToListAsync();

            return users;
        }

        public async Task<string[]> RolesAsync()
        {
            var roles = await this.data.Roles
                .OrderBy(r => r.Name)
                .Select(r => r.Name)
                .ToArrayAsync();

            return roles;
        }

        public async Task<IEnumerable<AdForListDTO>> AdsForApprovalAsync()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<Ad, AdForListDTO>().ForMember(
                    dto => dto.PhotoUrl, 
                    conf => conf.MapFrom(ol => ol.Photos.FirstOrDefault(p => p.IsMain && !p.IsDeleted).Url))
            );

            var ads = await this.data.Ads
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
            var user = await this.userManager.FindByNameAsync(userName);
            var userRoles = await this.userManager.GetRolesAsync(user);

            selectedRoles = selectedRoles ?? new string[] {};

            var result = await this.userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
            {
                return "Грешка при добавянето на роли.";
            }
            
            result = await this.userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
            {
                return "Грешка при премахването на роли.";
            }

            return true;
        }

        public async Task<Result> ApproveAdAsync(int id) 
        {
            var ad = await this.data.Ads
                .IgnoreQueryFilters()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (ad == null)
            {
                return "Обявата не е намерена";
            }

            ad.IsApproved = true;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
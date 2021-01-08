using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.Data.Models;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DataContext data, IMapper mapper)
            : base(data, mapper) {}

        public async Task<UserForDetailedDTO> GetWithAdsAsync(int id, bool includeNotApprovedAds)
        {
            var user = await GetUserAsync(id, includeNotApprovedAds);

            return this.mapper.Map<UserForDetailedDTO>(user);
        }

        private async Task<User> GetUserAsync(int id, bool includeNotApprovedAds)
        {
            var users = this.data.Users
                .Include(u => u.Ads)
                .ThenInclude(a => a.Photos)
                .Include(u => u.Ads)
                .ThenInclude(a => a.Category)
                .Include(u => u.Ads)
                .ThenInclude(a => a.User)
                .AsQueryable();
                
            if (includeNotApprovedAds)
            {
                users = users.IgnoreQueryFilters();
            }
            
            return await users
                .Where(u => u.Id == id && u.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<UserForUpdateDTO> DetailsAsync(int id)
        {
            return await this.data.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserForUpdateDTO>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Result> UpdateAsync(int id, UserForUpdateDTO model)
        {
            var result = await ValidateEmailAddress(id, model.Email);
            
            if (result.Failure)
            {
                return result.Error;
            }

            var user = await this.data.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            
            this.mapper.Map(model, user);

            return true;
        }

        private async Task<Result> ValidateEmailAddress(int userId, string email)
        {
            const int mainAdminId = 1;
            
            if (userId != mainAdminId && string.IsNullOrWhiteSpace(email))
            {
                return "Полето имейл не може да бъде празно";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmailAddress(email))
                {
                    return "Имейлът адресът не е валиден";
                }
                
                if (await EmailIsNotAvailableAsync(userId, email))
                {
                    return "Вече има регистриран потребител с този имейл адрес";
                }
            }

            return true;
        }

        private bool IsValidEmailAddress(string input)
        {
            try
            {
                var email = new MailAddress(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> EmailIsNotAvailableAsync(int userId, string email)
        {
            return await this.data.Users
                .AnyAsync(u => u.Id != userId && u.Email == email);
        }
    }
}
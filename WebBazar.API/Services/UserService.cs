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
            var user = await this.data.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return "Потребителят не е намерен";
            }
            
            this.mapper.Map(model, user);

            return true;
        }
    }
}
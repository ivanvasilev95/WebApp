using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data;
using WebApp.API.DTOs.User;
using WebApp.API.Helpers;
using WebApp.API.Models;
using WebApp.API.Services.Interfaces;

namespace WebApp.API.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DataContext context, IMapper mapper)
            : base(context, mapper) {}

        public async Task<Result<UserForDetailedDTO>> GetUserWithAdsAsync(int id, bool includeNotApprovedAds)
        {
            var user = await FindByIdAsync(id, includeNotApprovedAds);
            if(user == null)
            {
                return "Потребителят не е намерен";
            }

            return _mapper.Map<UserForDetailedDTO>(user);
        }

        private async Task<User> FindByIdAsync(int id, bool includeNotApprovedAds)
        {
            var query = _context
                .Users
                .Include(u => u.Ads)
                .ThenInclude(a => a.Photos)
                .Include(u => u.Ads)
                .ThenInclude(a => a.Category)
                .AsQueryable();

            if (includeNotApprovedAds)
            {
                query = query.IgnoreQueryFilters();
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<UserForUpdateDTO> GetUserForEditAsync(int id)
        {
            return await _context
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserForUpdateDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Result> UpdateUserAsync(int id, UserForUpdateDTO model)
        {
            var email = model.Email;

            // is not main admin (with id = 25) and email field is empty
            if (string.IsNullOrWhiteSpace(email) && id != 25)
            {
                return "Полето имейл не може да бъде празно";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmailAddress(email))
                {
                    return "Имейлът адресът не е валиден";
                }
                
                if (await EmailIsNotAvailableAsync(id, email))
                {
                    return "Вече има регистриран потребител с този имейл адрес";
                }
            }

            var user = await _context
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            
            _mapper.Map(model, user);

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
            return await _context.Users.AnyAsync(u => u.Id != userId && u.Email == email);
        }
    }
}
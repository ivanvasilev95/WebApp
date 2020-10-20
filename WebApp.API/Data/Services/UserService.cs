using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.User;
using WebApp.API.Extensions;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DataContext context, IMapper mapper)
            : base(context, mapper) {}

        public async Task<Result<UserForDetailedDTO>> GetUserWithAdsAsync(int id, ClaimsPrincipal currentUser)
        {
            var includeNotApprovedAds = this.IsUserEligible(id, currentUser);
            var user = await FindByIdAsync(id, includeNotApprovedAds);
            if(user == null)
            {
                return "Потребителят не е намерен";
            }

            return _mapper.Map<UserForDetailedDTO>(user);
        }

        private bool IsUserEligible(int userId, ClaimsPrincipal currentUser)
        {
            var currentUserId = int.Parse(currentUser.GetId() ?? "0");

            if (currentUser.IsAuthenticated())
            {
                var currentUserRoles = currentUser.GetUserRoles();
                return (currentUserId == userId || currentUserRoles.Contains("Admin") || currentUserRoles.Contains("Moderator"));
            }

            return false;
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
            
            // is not admin and email filed is empty
            if (string.IsNullOrWhiteSpace(email) && id != 1) // move this validations to FE except if email is not available
            {
                return "Полето имейл не може да бъде празно";
            }

            // logged user is admin and email field is not empty or is not admin
            if ((id == 1 && !string.IsNullOrWhiteSpace(email)) || id != 1)
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
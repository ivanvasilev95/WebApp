using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.User;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<UserForDetailedDTO>> ByIdAsync(int id, bool includeNotApprovedAds)
        {
            var user = await FindByIdAsync(id, includeNotApprovedAds);
            if(user == null)
            {
                return "Потребителят не е намерен";
            }

            return _mapper.Map<UserForDetailedDTO>(user);
        }

        public async Task<Result> UpdateAsync(int id, UserForUpdateDTO model)
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

            var user = await FindByIdAsync(id, false); // user.NormalizedEmail = userForUpdateDTO.Email?.ToUpper(); moved to mapper profiles
            
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

        private async Task<User> FindByIdAsync(int id, bool includeNotApprovedAds)
        {
            var query = _context.Users
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

        private async Task<bool> EmailIsNotAvailableAsync(int userId, string email)
        {
            return await _context.Users.AnyAsync(u => u.Id != userId && u.Email == email);
        }
    }
}
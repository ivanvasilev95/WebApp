using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.User;
using WebApp.API.Extensions;

namespace WebApp.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserRepository _userRepo;
        private readonly IPhotoRepository _photoRepo;
        private readonly IMapper _mapper;
        private readonly int _loggedInUserId;

        public UsersController(
            IUserRepository userRepo,
            IPhotoRepository photoRepo, 
            IMapper mapper)
        {
            _userRepo = userRepo;
            _photoRepo = photoRepo;
            _mapper = mapper;
            _loggedInUserId = int.Parse(this.User.GetId() ?? "0");
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            bool isLoggedInUserOrAdminOrModerator = IsUserEligible(id);
            var user = await _userRepo.GetUser(id, isLoggedInUserOrAdminOrModerator);
            if(user == null) {
                return NotFound("Потребителят не е намерен");
            }

            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);

            return Ok(userToReturn);
        }

        private bool IsUserEligible(int userId) {
            if (this.User.Identity.IsAuthenticated) {
                var loggedInUserRoles = ((ClaimsIdentity)this.User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                return (_loggedInUserId == userId || loggedInUserRoles.Contains("Admin") || loggedInUserRoles.Contains("Moderator"));
            }
            return false;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != _loggedInUserId)
                return Unauthorized();

            // userForUpdateDTO.Email = userForUpdateDTO.Email?.ToLower().Trim();
            await ValidateEmail(id, userForUpdateDTO.Email);

            var userFromRepo = await _userRepo.GetUser(id, false);
            userFromRepo.NormalizedEmail = userForUpdateDTO.Email?.ToUpper();
            _mapper.Map(userForUpdateDTO, userFromRepo);

            //if (await _userRepo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating user {id} failed on save.");
        }

        // user other than the main admin (with id = 1) must provide valid email address
        private async Task ValidateEmail(int userId, string email)
        {
            // is not admin and email filed is empty
            if (string.IsNullOrWhiteSpace(email) && userId != 1)
                throw new Exception("Полето имейл не може да бъде празно");

            // logged user is admin and email field is not empty or is not admin
            if ((userId == 1 && !string.IsNullOrWhiteSpace(email)) || userId != 1) {
                if (!IsValidEmailAddress(email)) {
                    throw new Exception("Имейлът адресът не е валиден");
                }
                
                if (await _userRepo.EmailIsNotAvailable(userId, email)) {
                    throw new Exception("Вече има регистриран потребител с този имейл адрес");
                }
            }
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
    }
}
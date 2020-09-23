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

namespace WebApp.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserRepository _userRepo;
        private readonly IPhotoRepository _photoRepo;
        private readonly IMapper _mapper;
        
        public UsersController(
            IUserRepository userRepo,
            IPhotoRepository photoRepo, 
            IMapper mapper)
        {
            _userRepo = userRepo;
            _photoRepo = photoRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            // bool isCurrentUserOrAdminOrModerator = IsUserEligible(id);

            // var user = await _userRepo.GetUser(id, isCurrentUserOrAdminOrModerator);
            var user = await _userRepo.GetUser(id, false);
            if(user == null) {
                return NotFound("Потребителят не е намерен");
            }

            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            foreach(var ad in userToReturn.Ads) {
                var photo = await _photoRepo.GetAdMainPhoto(ad.Id);
                ad.PhotoUrl = photo?.Url;
            }

            return Ok(userToReturn);
        }

        // private bool IsUserEligible(int userId){
        //     if (User.Identity.IsAuthenticated) {
        //         var currentUserRoles = ((ClaimsIdentity)User.Identity).Claims
        //             .Where(c => c.Type == ClaimTypes.Role)
        //             .Select(c => c.Value);
        //         return (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == userId || currentUserRoles.Contains("Admin") || currentUserRoles.Contains("Moderator"));
        //     }
        //     return false;
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            userForUpdateDTO.Email = userForUpdateDTO.Email?.Trim();
            
            var result = await ValidateEmail(userForUpdateDTO.Email, id);
            if (result != null)
                return BadRequest(result);

            var userFromRepo = await _userRepo.GetUser(id, false);
            userFromRepo.NormalizedEmail = userForUpdateDTO.Email?.ToUpper();
            _mapper.Map(userForUpdateDTO, userFromRepo);

            //if (await _userRepo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating user {id} failed on save.");
        }

        // user other than the main admin (with id = 1) must provide valid email address
        private async Task<string> ValidateEmail(string email, int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // is not admin and email filed is empty
            if (string.IsNullOrWhiteSpace(email) && currentUserId != 1)
                return "Полето имейл не може да бъде празно";

            // is admin and email field is not empty or is not admin
            if ((currentUserId == 1 && !string.IsNullOrWhiteSpace(email)) || currentUserId != 1) {
                if (!IsValidEmailAddress(email))
                    return "Имейлът не е валиден";
                
                if (await _userRepo.EmailIsNotAvailable(userId, email)) {
                    return "Вече има регистриран потребител с този имейл адрес";
                }
            }
            
            return null;
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
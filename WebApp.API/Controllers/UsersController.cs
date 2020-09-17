using System;
using System.Linq;
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
            bool isCurrentUserOrAdminOrModerator = isUserEligible(id);

            var user = await _userRepo.GetUser(id, isCurrentUserOrAdminOrModerator);
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

        private bool isUserEligible(int userId){
            if (User.Identity.IsAuthenticated) {
                var currentUserRoles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                return (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == userId || currentUserRoles.Contains("Admin") || currentUserRoles.Contains("Moderator"));
            }
            return false;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            if (await _userRepo.EmailIsNotAvailable(id, userForUpdateDTO.Email?.Trim())) {
                return BadRequest("Вече има регистриран потребител с този имейл адрес");
            }

            var userFromRepo = await _userRepo.GetUser(id, false);
            _mapper.Map(userForUpdateDTO, userFromRepo);

            //if (await _userRepo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating user {id} failed on save.");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using WebApp.API.DTOs;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IAdsRepository _adsRepo;
        private readonly IMapper _mapper;
        
        public UsersController(IUserRepository userRepo, IAdsRepository adsRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _adsRepo = adsRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            foreach(var ad in userToReturn.Ads)
                ad.PhotoUrl = _userRepo.getPhotoUrl(ad.Id);

            return Ok(userToReturn);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _userRepo.GetUser(id);

            _mapper.Map(userForUpdateDTO, userFromRepo);

            //if (await _repo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating user {id} failed on save.");
        }

        [HttpPost("{id}/like/{adId}")]
        public async Task<IActionResult> LikeAd(int id, int adId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var like = await _adsRepo.GetLike(id, adId);
            if (like != null) {
                return BadRequest("Обявата вече е добавена в Наблюдавани");
            }
            
            /*
            if(await _adsRepo.GetAd(adId) == null)
                return NotFound();
            */

            Ad ad = await _adsRepo.GetAd(adId);
            if(ad == null)
                return NotFound();
            if(ad.UserId == id)
                return BadRequest("Не може да добавяте собствени обяви в Наблюдавани");

            like = new Like {
                UserId = id,
                AdId = adId
            };

            _adsRepo.Add<Like>(like);

            if(await _adsRepo.SaveAll())
                return Ok();
            
            return BadRequest("Грешка при добавяне на обявата в Наблюдавани");
;        }
    }
}
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
    public class AdsController : ControllerBase
    {
        private readonly IAdsRepository _repo;
        private readonly IMapper _mapper;
        public AdsController(IAdsRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        [HttpGet("{id}/likes")]
        public IActionResult GetLikesCount(int id){
            int count = _repo.GetAdLikesCount(id);
            
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> GetAds()
        {
            var ads = await _repo.GetAds();
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAd(AdForCreateDTO adForCreateDTO)
        {
            var adToCreate = _mapper.Map<Ad>(adForCreateDTO);
            _repo.Add(adToCreate);
            var adToReturn = _mapper.Map<AdForDetailedDTO>(adToCreate);
            await _repo.SaveAll();

            return CreatedAtRoute("GetAd", new {controller = "Ads", id = adToCreate.Id}, adToReturn);
        }

        [HttpGet("{id}", Name = "GetAd")]
        public async Task<IActionResult> GetAd(int id)
        {
            var ad = await _repo.GetAd(id);
            var adToReturn = _mapper.Map<AdForDetailedDTO>(ad);
            adToReturn.CategoryName = _repo.GetAdCategoryName(adToReturn.CategoryId).Name;
     
            return Ok(adToReturn);
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ad>> RemoveAd(int id){
            var adToDelete = await _repo.GetAd(id);
            if(adToDelete == null)
                return NotFound();
            
            _repo.Delete(adToDelete);
            await _repo.SaveAll();

            return adToDelete;
        }

        [Authorize]
        [HttpDelete("user/{userId}/removes/{adId}")]
        public async Task<ActionResult<Like>> RemoveAdFromFavorites(int userId, int adId) {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
            var likeToRemove = await _repo.GetLike(userId, adId);
            if(likeToRemove == null)
                return NotFound();

            _repo.Delete(likeToRemove);
            await _repo.SaveAll();

            return likeToRemove;
        }

        [Authorize]
        [HttpGet]
        [Route("user")]
        public IActionResult GetUserAds(){ // GetLoggedUserAds
            int userId = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            var ads = _repo.GetUserAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }

        [Authorize]
        [HttpGet("user/{userId}/favorites")]
        public IActionResult GetUserFavorites(int userId){
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFavoriteAds = _repo.GetUserFavorites(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(userFavoriteAds);

            return Ok(adsToReturn);
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult GetAdCategories(){
            var categories = _repo.GetCategories();

            return Ok(categories);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAd(int id, AdForUpdateDTO adForUpdateDTO)
        {
            var adFromRepo = await _repo.GetAd(id);

            _mapper.Map(adForUpdateDTO, adFromRepo);

            //if (await _repo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating ad {id} failed on save.");
        }
    }
}
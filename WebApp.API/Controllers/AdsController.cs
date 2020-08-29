using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Ad;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class AdsController : ApiController
    {
        private readonly IAdsRepository _adsRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public AdsController(
            IAdsRepository adsRepo, 
            ICategoryRepository categoryRepo,
            IMapper mapper)
        {
            _adsRepo = adsRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAds([FromQuery]UserParams userParams)
        {
            var ads = await _adsRepo.GetAds(userParams);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            Response.AddPagination(ads.CurrentPage, ads.PageSize, ads.TotalCount, ads.TotalPages);

            return Ok(adsToReturn);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAd(AdForCreateDTO adForCreateDTO)
        {
            var adToCreate = _mapper.Map<Ad>(adForCreateDTO);
            _adsRepo.Add(adToCreate);
            await _adsRepo.SaveAll();

            var adToReturn = _mapper.Map<AdForDetailedDTO>(adToCreate); 
            adToReturn.CategoryName = await _categoryRepo.GetCategoryName(adToReturn.CategoryId);

            return CreatedAtRoute("GetAd", new {controller = "Ads", id = adToCreate.Id}, adToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetAd")]
        public async Task<IActionResult> GetAd(int id)
        {
            var ad = await _adsRepo.GetAd(id);
            if (ad == null)
                return NotFound("Обявата не е намерена");
                
            var adToReturn = _mapper.Map<AdForDetailedDTO>(ad);
            adToReturn.CategoryName = await _categoryRepo.GetCategoryName(adToReturn.CategoryId);
     
            return Ok(adToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Ad>> RemoveAd(int id) 
        {
            var adToDelete = await _adsRepo.GetAd(id);
            if(adToDelete == null)
                return NotFound();
            
            _adsRepo.Delete(adToDelete);
            await _adsRepo.SaveAll();

            return adToDelete;
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetUserAds()
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            var ads = await _adsRepo.GetUserAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }

        [HttpGet("user/{userId}/favorites")]
        public async Task<IActionResult> GetUserFavoriteAds(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFavoriteAds = await _adsRepo.GetUserLikedAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(userFavoriteAds);

            return Ok(adsToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAd(int id, AdForUpdateDTO adForUpdateDTO)
        {
            var adFromRepo = await _adsRepo.GetAd(id);

            _mapper.Map(adForUpdateDTO, adFromRepo);

            //if (await _adsRepo.SaveAll())
                return NoContent();
            
            //throw new Exception($"Updating ad {id} failed on save.");
        }
    }
}
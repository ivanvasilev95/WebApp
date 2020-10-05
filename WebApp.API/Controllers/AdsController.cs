using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Ad;
using WebApp.API.Extensions;
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
            IMapper mapper,
            DataContext context)
        {
            _adsRepo = adsRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
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
            await _adsRepo.Add(adToCreate);
            await _adsRepo.SaveAll();

            var adToReturn = _mapper.Map<AdForDetailedDTO>(await _adsRepo.GetAd(adToCreate.Id));

            return CreatedAtRoute("GetAd", new {controller = "Ads", id = adToCreate.Id}, adToReturn);
        }

        [HttpGet("{id}", Name = "GetAd")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAd(int id)
        {
            var ad = await _adsRepo.GetAd(id);
            if (ad == null)
                return NotFound("Обявата не е намерена");
     
            return Ok(_mapper.Map<Ad, AdForDetailedDTO>(ad));
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

        [HttpGet("personal")]
        public async Task<IActionResult> GetUserAds()
        {
            int userId = int.Parse(this.User.GetId());
            var ads = await _adsRepo.GetUserAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }

        [HttpGet("liked")]
        public async Task<IActionResult> GetUserLikedAds()
        {
            int userId = int.Parse(this.User.GetId());
            var userLikedAds = await _adsRepo.GetUserLikedAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(userLikedAds);

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
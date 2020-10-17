using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Ad;
using WebApp.API.Extensions;
using WebApp.API.Helpers;

namespace WebApp.API.Controllers
{
    public class AdsController : ApiController
    {
        private readonly IAdService _adService;
        
        public AdsController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]UserParams userParams)
        {
            var ads = await _adService.AllAsync(userParams, this.Response);

            return Ok(ads);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _adService.ByIdAsync(id);
            if (result.Failure)
                return NotFound(result.Error);
     
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdForCreateDTO adForCreateDTO)
        {
           var id = await _adService.CreateAsync(adForCreateDTO);

            return Created(nameof(this.Create), id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var result = await _adService.DeleteAsync(id);
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("personal")]
        public async Task<IActionResult> Mine()
        {
            int userId = int.Parse(this.User.GetId());

            var ads = await _adService.UserAdsAsync(userId);

            return Ok(ads);
        }

        [HttpGet("liked")]
        public async Task<IActionResult> Liked()
        {
            int userId = int.Parse(this.User.GetId());

            var ads = await _adService.UserLikedAdsAsync(userId);

            return Ok(ads);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AdForUpdateDTO adForUpdateDTO)
        {
            var result = await _adService.UpdateAsync(id, adForUpdateDTO);
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
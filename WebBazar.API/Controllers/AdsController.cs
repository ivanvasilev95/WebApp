using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Helpers;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Controllers
{
    public class AdsController : ApiController
    {
        private readonly IAdService _adService;
        private readonly ICurrentUserService _currentUser;
        
        public AdsController(IAdService adService, ICurrentUserService currentUser)
        {
            _adService = adService;
            _currentUser = currentUser;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AdParams adParams)
        {
            var ads = await _adService.AllAsync(adParams);

            return Ok(ads);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _adService.ByIdAsync(id);

            if (result.Failure)
            {
                return NotFound(result.Error);
            }

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
            var userId = _currentUser.GetId();

            var ads = await _adService.UserAdsAsync(userId);

            return Ok(ads);
        }

        [HttpGet("liked")]
        public async Task<IActionResult> Liked()
        {
            var userId = _currentUser.GetId();

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
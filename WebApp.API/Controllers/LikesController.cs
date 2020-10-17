using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.Extensions;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }
        
        [HttpPost]
        public async Task<IActionResult> LikeAd([FromQuery]int adId)
        {
            var userId = int.Parse(this.User.GetId());
            
            var result = await _likeService.Like(userId, adId);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<Like>> UnlikeAd([FromQuery]int adId) 
        {      
            var userId = int.Parse(this.User.GetId());
            
            var result = await _likeService.Unlike(userId, adId);
            if (result.Failure)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAdLikesCount([FromQuery]int adId)
        {
            var count = await _likeService.AdLikesCount(adId);

            return Ok(count);
        }
    }
}
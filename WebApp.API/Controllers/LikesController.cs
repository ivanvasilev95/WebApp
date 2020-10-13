using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.Extensions;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly ILikeService _likes;

        public LikesController(ILikeService likes)
        {
            _likes = likes;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> LikeAd([FromQuery]int adId)
        {
            var loggedInUserId = int.Parse(this.User.GetId());
            
            var result = await _likes.Like(loggedInUserId, adId);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<Like>> RemoveAdFromLiked([FromQuery]int adId) 
        {      
            var loggedInUserId = int.Parse(this.User.GetId());
            
            var result = await _likes.Unlike(loggedInUserId, adId);
            if (result.Failure)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAdLikesCount([FromQuery]int adId)
        {
            var count = await _likes.AdLikesCount(adId);

            return Ok(count);
        }
    }
}
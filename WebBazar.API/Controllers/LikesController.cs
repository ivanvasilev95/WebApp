using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Data.Models;

namespace WebBazar.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly ILikeService _likeService;
        private readonly ICurrentUserService _currentUser;

        public LikesController(ILikeService likeService, ICurrentUserService currentUser)
        {
            _likeService = likeService;
            _currentUser = currentUser;
        }
        
        [HttpPost("like")]
        public async Task<IActionResult> LikeAd([FromQuery]int adId)
        {
            var userId = _currentUser.GetId();
            
            var result = await _likeService.Like(userId, adId);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("unlike")]
        public async Task<ActionResult<Like>> UnlikeAd([FromQuery]int adId) 
        {      
            var userId = _currentUser.GetId();

            var result = await _likeService.Unlike(userId, adId);

            if (result.Failure)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
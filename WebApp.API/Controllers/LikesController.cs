using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.Extensions;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly IAdsRepository _adsRepo;
        private readonly ILikesRepository _likesRepo;
        private readonly int _loggedInUserId;

        public LikesController(
            IAdsRepository adsRepo, 
            ILikesRepository likesRepo)
        {
            _adsRepo = adsRepo;
            _likesRepo = likesRepo;
            _loggedInUserId = int.Parse(this.User.GetId());
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> LikeAd([FromQuery]int adId)
        {   
            var like = await _likesRepo.GetLike(_loggedInUserId, adId);
            if (like != null) {
                return BadRequest("Обявата вече е добавена в Наблюдавани");
            }

            var ad = await _adsRepo.GetAd(adId);
            if(ad == null)
                return NotFound("Обявата не е намерена");

            if(ad.UserId == _loggedInUserId)
                return BadRequest("Не може да добавяте собствени обяви в Наблюдавани");

            like = new Like {
                UserId = _loggedInUserId,
                AdId = adId
            };

            await _likesRepo.Add(like);

            if(await _likesRepo.SaveAll())
                return Ok();
            
            return BadRequest("Грешка при добавяне на обявата в Наблюдавани");
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<Like>> RemoveAdFromLiked([FromQuery]int adId) 
        {       
            var likeToRemove = await _likesRepo.GetLike(_loggedInUserId, adId);
            if(likeToRemove == null)
                return NotFound();

            _likesRepo.Delete(likeToRemove);
            await _likesRepo.SaveAll();

            return likeToRemove;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAdLikesCount([FromQuery]int adId)
        {
            var count = await _likesRepo.GetAdLikesCount(adId);
            return Ok(count);
        }
    }
}
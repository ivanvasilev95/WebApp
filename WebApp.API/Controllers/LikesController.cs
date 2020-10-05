using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly IAdsRepository _adsRepo;
        private readonly ILikesRepository _likesRepo;
        
        public LikesController(
            IAdsRepository adsRepo, 
            ILikesRepository likesRepo)
        {
            _adsRepo = adsRepo;
            _likesRepo = likesRepo;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> LikeAd([FromQuery]int adId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var like = await _likesRepo.GetLike(userId, adId);
            if (like != null) {
                return BadRequest("Обявата вече е добавена в Наблюдавани");
            }

            var ad = await _adsRepo.GetAd(adId);
            if(ad == null)
                return NotFound("Обявата не е намерена");

            if(ad.UserId == userId)
                return BadRequest("Не може да добавяте собствени обяви в Наблюдавани");

            like = new Like {
                UserId = userId,
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
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
            var likeToRemove = await _likesRepo.GetLike(userId, adId);
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
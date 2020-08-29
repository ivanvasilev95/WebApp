using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPost("user/{userId}/ad/{adId}")]
        public async Task<IActionResult> LikeAd(int userId, int adId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
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

            _likesRepo.Add(like);

            if(await _likesRepo.SaveAll())
                return Ok();
            
            return BadRequest("Грешка при добавяне на обявата в Наблюдавани");
        }

        [HttpDelete("remove/user/{userId}/ad/{adId}")]
        public async Task<ActionResult<Like>> RemoveAdFromFavorites(int userId, int adId) 
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
                
            var likeToRemove = await _likesRepo.GetLike(userId, adId);
            if(likeToRemove == null)
                return NotFound();

            _likesRepo.Delete(likeToRemove);
            await _likesRepo.SaveAll();

            return likeToRemove;
        }

        [AllowAnonymous]
        [HttpGet("count/ad/{adId}")]
        public async Task<IActionResult> GetAdLikesCount(int adId)
        {
            var count = await _likesRepo.GetAdLikesCount(adId);
            return Ok(count);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    public class LikesController : ApiController
    {
        private readonly ILikeService likes;
        private readonly ICurrentUserService currentUser;

        public LikesController(ILikeService likes, ICurrentUserService currentUser)
        {
            this.likes = likes;
            this.currentUser = currentUser;
        }
        
        [HttpPost(nameof(Like))]
        public async Task<ActionResult> Like([FromQuery]int adId)
        {
            return await this.likes
                .LikeAsync(adId, this.currentUser.GetId())
                .ToActionResult();
        }

        [HttpDelete(nameof(Unlike))]
        public async Task<ActionResult> Unlike([FromQuery]int adId) 
        {      
            return await this.likes
                .UnlikeAsync(adId, this.currentUser.GetId())
                .ToActionResult();
        }
    }
}
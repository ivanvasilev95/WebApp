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
        
        [HttpPost(nameof(LikeAd) + PathSeparator + Id)]
        public async Task<ActionResult> LikeAd(int id)
        {
            return await this.likes
                .LikeAdAsync(id, this.currentUser.GetId())
                .ToActionResult();
        }

        [HttpDelete(nameof(UnlikeAd) + PathSeparator + Id)]
        public async Task<ActionResult> UnlikeAd(int id) 
        {      
            return await this.likes
                .UnlikeAdAsync(id, this.currentUser.GetId())
                .ToActionResult();
        }
    }
}
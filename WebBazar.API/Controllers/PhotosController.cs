using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.Photo;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    public class PhotosController : ApiController
    {
        private readonly IPhotoService photos;

        public PhotosController(IPhotoService photos)
        {
            this.photos = photos;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm]PhotoForCreationDTO model, [FromQuery]int adId)
        {
            var result = await this.photos.AddAsync(model, adId);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.Add), result.Data);
        }
        
        [HttpPut(nameof(SetMain) + PathSeparator + Id)]
        public async Task<ActionResult> SetMain(int id)
        {
            return await this.photos
                .SetMainAsync(id)
                .ToActionResult();
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            return await this.photos
                .DeleteAsync(id)
                .ToActionResult();
        }
    }
}
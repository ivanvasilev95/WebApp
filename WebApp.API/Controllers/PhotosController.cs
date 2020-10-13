using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Photo;

namespace WebApp.API.Controllers
{
    public class PhotosController : ApiController
    {
        private readonly IPhotoService _photos;

        public PhotosController(IPhotoService photos)
        {
            _photos = photos;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var result = await _photos.ByIdAsync(id);
            if (result.Failure)
                return NotFound(result.Error);
     
            return Ok(result.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPhoto([FromQuery]int adId, [FromForm]PhotoForCreationDTO photoForCreationDTO)
        {
            var result = await _photos.CreateAsync(adId, photoForCreationDTO);
            if (result.Failure)
                return BadRequest(result.Error);

            // test it
            return CreatedAtRoute("GetPhoto", new {controller = "Photos", id = result.Data.Id}, result.Data);
        }
        
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int id)
        {
            var result = await _photos.SetMainAsync(id);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePhoto(int id)
        {
            var result = await _photos.DeleteAsync(id);
            if (result.Failure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}
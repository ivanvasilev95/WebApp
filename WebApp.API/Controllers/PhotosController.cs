using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Photo;

namespace WebApp.API.Controllers
{
    public class PhotosController : ApiController
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery]int adId, [FromForm]PhotoForCreationDTO photoForCreationDTO)
        {
            var result = await _photoService.AddAsync(adId, photoForCreationDTO);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.Add), result.Data);
        }
        
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int id)
        {
            var result = await _photoService.SetMainAsync(id);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _photoService.DeleteAsync(id);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
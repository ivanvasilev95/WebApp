using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.API.Data;
using WebApp.API.DTOs;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("ads/{adId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdsRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IMapper mapper, IAdsRepository repo,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;
            _mapper = mapper;

            Account acc = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc); 
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id) {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDTO>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForAd(int adId, [FromForm]PhotoForCreationDTO photoForCreationDTO){
            var adFromRepo = await _repo.GetAd(adId);
            var file = photoForCreationDTO.File;

            if(file == null)
                return BadRequest();

            var uploadResult = new ImageUploadResult();
            
            if(file.Length > 0) {
                using(var stream = file.OpenReadStream()){
                    var uploadParams = new ImageUploadParams() {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);

            if(!adFromRepo.Photos.Any(a => a.IsMain))
                photo.IsMain = true;
            
            adFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll()){
                var photoToReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }
            
            return BadRequest("Не може да се добави снимката");
        }

        [HttpPost("{id}/SetMain")]
        public async Task<IActionResult> SetMainPhoto(int adId, int id) {
            var ad = await _repo.GetAd(adId);

            if(!ad.Photos.Any(p => p.Id == id))
                return BadRequest("Обявата няма снимка с такова id");
            
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("Тази снимка вече е зададена като главна");
            
            var currentMainPhoto = await _repo.GetMainPhotoForAd(adId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if(await _repo.SaveAll())
                return NoContent();
            
            return BadRequest("Не може да се зададе снимката като главна");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int adId, int id) {
            var ad = await _repo.GetAd(adId);

            if(!ad.Photos.Any(p => p.Id == id))
                return BadRequest("Обявата няма снимка с такова id");
            
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("Тази снимка вече е зададена като главна");
            
            if(photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if(result.Result == "ok") {
                    _repo.Delete(photoFromRepo);
                }
            }

            if(photoFromRepo.PublicId == null)
                _repo.Delete(photoFromRepo);

            if(await _repo.SaveAll())
                return Ok();
            
            return BadRequest("Грешка при изтриване на снимката");
        }
    }
}
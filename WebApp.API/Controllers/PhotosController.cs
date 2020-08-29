using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Photo;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("ads/{adId}/photos")]
    [ServiceFilter(typeof(LogUserActivity))]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdsRepository _adsRepo;
        private readonly IPhotoRepository _photoRepo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(
            IMapper mapper, 
            IAdsRepository adsRepo,
            IPhotoRepository photoRepo,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _adsRepo = adsRepo;
            _photoRepo = photoRepo;
            _mapper = mapper;

            Account acc = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc); 
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _photoRepo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDTO>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int adId, [FromForm]PhotoForCreationDTO photoForCreationDTO)
        {
            var file = photoForCreationDTO.File;

            if(file == null)
                return BadRequest();

            var uploadResult = UploadToCloudinary(file);

            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);

            var adFromRepo = await _adsRepo.GetAd(adId);
            if(!adFromRepo.Photos.Any(a => a.IsMain))
                photo.IsMain = true;
            
            adFromRepo.Photos.Add(photo);

            if(await _adsRepo.SaveAll()){
                var photoToReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }
            
            return BadRequest("Не може да се добави снимката");
        }
        
        private ImageUploadResult UploadToCloudinary(IFormFile file) 
        {
            var uploadResult = new ImageUploadResult();

            if(file.Length > 0) 
            {
                using (var stream = file.OpenReadStream()) 
                {
                    var uploadParams = new ImageUploadParams() 
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            return uploadResult;
        }

        [HttpPost("{id}/SetMain")]
        public async Task<IActionResult> SetMainPhoto(int adId, int id)
        {
            var ad = await _adsRepo.GetAd(adId);

            if(!ad.Photos.Any(p => p.Id == id))
                return BadRequest("Обявата няма снимка с такова id");
            
            var photoFromRepo = await _photoRepo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("Тази снимка вече е зададена като главна");
            
            var currentMainPhoto = await _photoRepo.GetAdMainPhoto(adId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if(await _photoRepo.SaveAll())
                return NoContent();
            
            return BadRequest("Не може да се зададе снимката като главна");
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int adId, int photoId)
        {
            var ad = await _adsRepo.GetAd(adId);

            if(!ad.Photos.Any(p => p.Id == photoId))
                return BadRequest("Обявата няма снимка с такова id");
            
            var photoFromRepo = await _photoRepo.GetPhoto(photoId);
            if(photoFromRepo.IsMain)
                return BadRequest("Тази снимка е зададена като главна и не може да се изтрие");
            
            if(photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if(result.Result == "ok") {
                    _photoRepo.Delete(photoFromRepo);
                }
            } 
            else 
            {
                _photoRepo.Delete(photoFromRepo);
            }

            if(await _photoRepo.SaveAll())
                return Ok();
            
            return BadRequest("Грешка при изтриване на снимката");
        }
    }
}
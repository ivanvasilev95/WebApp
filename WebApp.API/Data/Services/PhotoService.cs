using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Photo;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Services
{
    public class PhotoService : BaseService, IPhotoService
    {
        private Cloudinary _cloudinary;

        public PhotoService(DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
            : base(context, mapper)
        {
            Account acc = new Account(cloudinaryConfig.Value.CloudName, cloudinaryConfig.Value.ApiKey, cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc); 
        }

        public async Task<Result<PhotoForReturnDTO>> AddAsync(int adId, PhotoForCreationDTO model)
        {
            var file = model.File;

            if (file == null)
            {
                return "Снимката не е намерена";
            }

            var uploadResult = UploadToCloudinary(file);

            model.Url = uploadResult.Uri.ToString();
            model.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(model);

            var ad = await _context
                .Ads
                .Where(a => a.Id == adId)
                .FirstOrDefaultAsync();

            if (!ad.Photos.Any(p => p.IsMain))
                photo.IsMain = true;
            
            ad.Photos.Add(photo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return _mapper.Map<Photo, PhotoForReturnDTO>(photo);
            }
            
            return "Не може да се добави снимката";
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

        public async Task<Result> DeleteAsync(int id)
        {
            var photoFromRepo = await GetPhotoAsync(id);
            if (photoFromRepo.IsMain)
                return "Тази снимка е зададена като главна и не може да се изтрие";
            
            DeletePhoto(photoFromRepo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            
            return "Грешка при изтриване на снимката";
        }

        private void DeletePhoto(Photo photo) 
        {
            if (photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _context.Photos.Remove(photo);
                }
            }
            else 
            {
                _context.Photos.Remove(photo);
            }
        }

        public async Task<Result> SetMainAsync(int id)
        {
            var photo = await GetPhotoAsync(id);

            if (photo.IsMain)
                return "Тази снимка вече е зададена като главна";
            
            var currentMainPhoto = await _context
                .Photos
                .FirstOrDefaultAsync(p => p.AdId == photo.AdId && p.IsMain);
                
            currentMainPhoto.IsMain = false;
            photo.IsMain = true;

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            
            return "Не може да се зададе снимката като главна";
        }

        private async Task<Photo> GetPhotoAsync(int id)
        {
            return await _context
                .Photos
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
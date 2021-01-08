using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Photo;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services
{
    public class PhotoService : BaseService, IPhotoService
    {
        private Cloudinary cloudinary;

        public PhotoService(
            DataContext data,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
            : base(data, mapper)
        {
            var account = new Account(cloudinaryConfig.Value.CloudName, cloudinaryConfig.Value.ApiKey, cloudinaryConfig.Value.ApiSecret);
            this.cloudinary = new Cloudinary(account); 
        }

        public async Task<Result<PhotoForDetailedDTO>> AddAsync(PhotoForCreationDTO model, int adId)
        {
            var file = model.File;

            if (file == null)
            {
                return "Снимката не е намерена";
            }

            var uploadResult = UploadToCloudinary(file);

            model.Url = uploadResult.Uri.ToString();
            model.PublicId = uploadResult.PublicId;

            var photo = this.mapper.Map<Photo>(model);

            var ad = await this.data.Ads
                .Include(a => a.Photos)
                .IgnoreQueryFilters()
                .Where(a => a.Id == adId && a.IsDeleted == false)
                .FirstOrDefaultAsync();
            
            var adAlreadyHasMainPhoto = ad.Photos.Any(p => p.IsMain && !p.IsDeleted);

            if (!adAlreadyHasMainPhoto)
            {
                photo.IsMain = true;
            }
            
            ad.Photos.Add(photo);

            await this.data.SaveChangesAsync();
            
            return this.mapper.Map<Photo, PhotoForDetailedDTO>(photo);
        }

        private ImageUploadResult UploadToCloudinary(IFormFile file) 
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0) 
            {
                using (var stream = file.OpenReadStream()) 
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                    };

                    uploadResult = this.cloudinary.Upload(uploadParams);
                }
            }

            return uploadResult;
        }

        public async Task<Result> SetMainAsync(int id)
        {
            var photo = await GetPhotoAsync(id);

            if (photo.IsMain)
            {
                return "Тази снимка вече е зададена като главна";
            }
            
            var currentMainPhoto = await this.data.Photos
                .Where(p => p.AdId == photo.AdId && p.IsMain)
                .FirstOrDefaultAsync();
                
            currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            await this.data.SaveChangesAsync();
                
            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var photo = await GetPhotoAsync(id);

            if (photo.IsMain)
            {
                return "Тази снимка е зададена като главна и не може да се изтрие";  
            }
            
            DeletePhoto(photo);

            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<Photo> GetPhotoAsync(int id)
        {
            return await this.data.Photos
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        private void DeletePhoto(Photo photo) 
        {
            if (photo.PublicId != null)
            {
                var deletionParams = new DeletionParams(photo.PublicId);

                var deletionResult = this.cloudinary.Destroy(deletionParams);

                if (deletionResult.Result == "ok")
                {
                    this.data.Remove(photo);
                }
            }
            else 
            {
                this.data.Remove(photo);
            }
        }
    }
}
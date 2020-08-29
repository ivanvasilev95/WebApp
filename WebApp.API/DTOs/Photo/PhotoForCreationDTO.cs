using Microsoft.AspNetCore.Http;

namespace WebApp.API.DTOs.Photo
{
    public class PhotoForCreationDTO
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
    }
}
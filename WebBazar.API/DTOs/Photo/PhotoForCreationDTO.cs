using Microsoft.AspNetCore.Http;

namespace WebBazar.API.DTOs.Photo
{
    public class PhotoForCreationDTO
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        public IFormFile File { get; set; }
    }
}
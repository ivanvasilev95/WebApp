using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.Category
{
    public class CategoryForCreationDTO
    {
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
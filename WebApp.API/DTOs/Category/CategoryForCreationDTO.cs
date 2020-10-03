using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.Category
{
    public class CategoryForCreationDTO
    {
        [Required(ErrorMessage = "Името на категорията е задължително")]
        [MinLength(4, ErrorMessage = "Името на категорията не трябва да бъде по-малко от 4 символа")]
        [MaxLength(20, ErrorMessage = "Името на категорията не трябва да надвишава 20 символа")]
        public string Name { get; set; }
    }
}
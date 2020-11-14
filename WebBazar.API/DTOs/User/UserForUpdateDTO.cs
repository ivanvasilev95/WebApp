// using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.User
{
    public class UserForUpdateDTO
    {
        // [Required]
        public string UserName { get; set; }

        // [Required(ErrorMessage = "тест")]
        public string FullName { get; set; }

        // [Required(ErrorMessage = "тест2")]
        public string Address { get; set; }
        
        // [Required(ErrorMessage = "тест3")]
        // [EmailAddress(ErrorMessage = "Имейл адресът не е валиден")]
        public string Email { get; set; }
    }
}
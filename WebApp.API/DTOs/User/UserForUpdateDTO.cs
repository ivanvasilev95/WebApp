// using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.User
{
    public class UserForUpdateDTO
    {
        // [Required]
        public string UserName { get; set; }

        // [Required]
        public string FullName { get; set; }

        // [Required]
        public string Address { get; set; }
        
        // [Required]
        // [EmailAddress(ErrorMessage = "Имейл адресът не е валиден")]
        public string Email { get; set; }
    }
}
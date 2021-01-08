using System;
using System.ComponentModel.DataAnnotations;

namespace WebBazar.API.DTOs.User
{
    public class UserForRegisterDTO
    {
        public UserForRegisterDTO() 
        {
            this.LastActive = DateTime.Now;
            this.CreatedOn = DateTime.Now;       
        }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Паролата трябва да бъде между 6 и 12 символа.")]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Имейл адресът не е валиден")]
        public string Email { get; set; }
        
        [Required]
        public string FullName { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public DateTime CreatedOn { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.User
{
    public class UserForRegisterDTO
    {
        public UserForRegisterDTO() 
        {
            this.Created = DateTime.Now;
            this.LastActive = DateTime.Now;       
        }
        
        [Required]
        public string Username { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Паролата трябва да бъде между 6 и 12 символа.")]
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}
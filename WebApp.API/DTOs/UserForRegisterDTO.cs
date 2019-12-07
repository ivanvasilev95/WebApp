using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs
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
        [StringLength(8, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 12 characters.")]
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}
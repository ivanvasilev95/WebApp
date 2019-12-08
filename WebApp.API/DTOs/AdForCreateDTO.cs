using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs
{
    public class AdForCreateDTO
    {
        public AdForCreateDTO() 
        {
            this.DateAdded = DateTime.Now;   
        }
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public bool IsUsed { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
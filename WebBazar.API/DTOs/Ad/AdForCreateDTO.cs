using System;
using System.ComponentModel.DataAnnotations;

namespace WebBazar.API.DTOs.Ad
{
    public class AdForCreateDTO
    {
        public AdForCreateDTO() 
        {
            this.DateAdded = DateTime.Now;   
        }
        
        [Required]
        public string Title { get; set; }
        // [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        public double? Price { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
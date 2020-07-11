using System;
using System.Collections.Generic;
using WebApp.API.Models;

namespace WebApp.API.DTOs
{
    public class AdForDetailedDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double? Price { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDTO> Photos { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsApproved { get; set; }
    }
}
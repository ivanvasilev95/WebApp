using System;
using System.Collections.Generic;

namespace WebApp.API.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double? Price { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsApproved { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
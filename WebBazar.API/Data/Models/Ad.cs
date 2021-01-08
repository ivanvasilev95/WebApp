using System;
using System.Collections.Generic;
using WebBazar.API.Data.Models.Base;

namespace WebBazar.API.Data.Models
{
    public class Ad : DeletableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool? IsUsed { get; set; }
        public double? Price { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Message> Messages { get; set; }  
    }
}
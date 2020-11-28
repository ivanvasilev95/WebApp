using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.API.Data.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<Ad> Ads { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
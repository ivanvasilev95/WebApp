using System;
using System.Collections.Generic;
using WebApp.API.Models;

namespace WebApp.API.DTOs
{
    public class UserForDetailedDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }   
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<Ad> Ads { get; set; }
    }
}
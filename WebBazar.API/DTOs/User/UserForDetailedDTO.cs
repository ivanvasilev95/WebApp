using System;
using System.Collections.Generic;
using WebBazar.API.DTOs.Ad;

namespace WebBazar.API.DTOs.User
{
    public class UserForDetailedDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }   
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<AdForDetailedDTO> Ads { get; set; }
    }
}
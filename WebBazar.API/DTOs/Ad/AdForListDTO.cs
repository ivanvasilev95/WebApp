using System;

namespace WebBazar.API.DTOs.Ad
{
    public class AdForListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string PhotoUrl { get; set; }
        public double? Price { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
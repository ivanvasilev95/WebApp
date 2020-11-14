using System.Collections.Generic;

namespace WebApp.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ad> Ads { get; set; }
    }
}
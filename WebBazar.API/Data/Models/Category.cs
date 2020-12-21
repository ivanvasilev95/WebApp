using System.Collections.Generic;
using WebBazar.API.Data.Models.Base;

namespace WebBazar.API.Data.Models
{
    public class Category : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ad> Ads { get; set; }
    }
}
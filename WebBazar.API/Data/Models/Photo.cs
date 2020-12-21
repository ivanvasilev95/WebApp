using WebBazar.API.Data.Models.Base;

namespace WebBazar.API.Data.Models
{
    public class Photo : DeletableEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public Ad Ad { get; set; }
        public int AdId { get; set; }
    }
}
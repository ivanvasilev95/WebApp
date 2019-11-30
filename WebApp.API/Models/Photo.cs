namespace WebApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public Ad Ad { get; set; }
        public int AdId { get; set; }
    }
}
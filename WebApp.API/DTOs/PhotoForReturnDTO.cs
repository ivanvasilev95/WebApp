namespace WebApp.API.DTOs
{
    public class PhotoForReturnDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}
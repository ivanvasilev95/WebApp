namespace WebBazar.API.DTOs.Ad
{
    public class AdForUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int CategoryId { get; set; }
        public bool? IsUsed { get; set; }
        public double? Price { get; set; }
    }
}
namespace WebApp.API.Data.Models
{
    public class Like
    {   
        public int UserId { get; set; }
        public int AdId { get; set; }
        public User User { get; set; }
        public Ad Ad { get; set; }
    }
}
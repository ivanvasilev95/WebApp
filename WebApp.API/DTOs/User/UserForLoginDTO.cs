namespace WebApp.API.DTOs.User
{
    public class UserForLoginDTO
    {
        // [Required]
        public string Username { get; set; }
        
        // [Required]
        public string Password { get; set; }
    }
}
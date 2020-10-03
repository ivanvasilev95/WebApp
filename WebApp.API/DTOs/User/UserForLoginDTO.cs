namespace WebApp.API.DTOs.User
{
    public class UserForLoginDTO
    {
        // [Required]
        public string UserName { get; set; }
        
        // [Required]
        public string Password { get; set; }
    }
}
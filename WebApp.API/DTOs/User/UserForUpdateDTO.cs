using System.ComponentModel.DataAnnotations;

namespace WebApp.API.DTOs.User
{
    public class UserForUpdateDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        
        // [EmailAddress(ErrorMessage = "Имейл адресът не е валиден")]
        public string Email { get; set; }
    }
}
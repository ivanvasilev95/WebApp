using WebBazar.API.Infrastructure;

namespace WebBazar.API.DTOs.User
{
    public class UserForUpdateDTO
    {
        public string UserName { get; set; }

        [ValidEmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
using System.Collections.Generic;

namespace WebBazar.API.DTOs.User
{
    public class UserWithRolesServiceModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
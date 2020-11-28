using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebBazar.API.Data.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
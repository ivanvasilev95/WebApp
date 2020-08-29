using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            var roles = new List<Role>
            {
                new Role {Name = "Member"},
                new Role {Name = "Admin"},
                new Role {Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }

            var adminUser = new User { UserName = "admin" };

            IdentityResult result = _userManager.CreateAsync(adminUser, "admin").Result;
            if (result.Succeeded) {
                var admin = _userManager.FindByNameAsync("admin").Result;
                _userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
            }
        }
    }
}
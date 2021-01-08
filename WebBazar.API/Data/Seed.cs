using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebBazar.API.Data.Models;

namespace WebBazar.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            var roles = new List<Role>
            {   
                new Role {Name = "Admin"},
                new Role {Name = "Moderator"},
                new Role {Name = "Member"}
            };

            foreach (var role in roles)
            {
                this.roleManager.CreateAsync(role).Wait();
            }

            var adminUser = new User { UserName = "admin" };

            IdentityResult result = this.userManager.CreateAsync(adminUser, "admin1").Result;
            
            if (result.Succeeded)
            {
                var admin = this.userManager.FindByNameAsync("admin").Result;
                this.userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
        }
    }
}
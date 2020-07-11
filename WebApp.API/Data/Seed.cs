using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
                new Role{Name = "Member"},
                new Role{Name = "Admin"},
                new Role{Name = "Moderator"},
                new Role{Name = "VIP"}
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

            /*
            if (!_userManager.Users.Any()) {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users) {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }
            }
            */
        }
        /*
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;

        }
        public void SeedAds()
        {
            if (!_context.Ads.Any()) {
                var adData = System.IO.File.ReadAllText("Data/AdSeedData.json");
                var ads = JsonConvert.DeserializeObject<List<Ad>>(adData);
                foreach (var ad in ads) {
                    _context.Ads.Add(ad);
                }
                _context.SaveChanges(); 
            }
        }
        */
    }
}
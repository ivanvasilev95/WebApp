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
        public Seed(UserManager<User> userManager)
        {
            _userManager = userManager;

        }
        public void SeedUsers()
        {
            if (!_userManager.Users.Any()) {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users) {
                    _userManager.CreateAsync(user, "password").Wait();
                }
            }
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
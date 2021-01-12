using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
        {
            this.users = users;
        }

        [AllowAnonymous]
        [HttpGet(Id)]
        public async Task<UserForDetailedDTO> UserWithAds([FromRoute]int id, [FromQuery]bool includeNotApprovedAds)
        {
            return await this.users.GetWithAdsAsync(id, includeNotApprovedAds);
        }

        [HttpGet(nameof(Details) + PathSeparator + Id)]
        public async Task<UserForUpdateDTO> Details(int id)
        {
            return await this.users.DetailsAsync(id);
        }

        [HttpPut(nameof(Update) + PathSeparator + Id)]
        public async Task<ActionResult> Update(int id, UserForUpdateDTO model)
        {
            return await this.users
                .UpdateAsync(id, model)
                .ToActionResult();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.DTOs.Role;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Extensions;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdminService admin;

        public AdminController(IAdminService admin)   
        {
            this.admin = admin;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(nameof(UsersWithRoles))]
        public async Task<IEnumerable<UserWithRolesServiceModel>> UsersWithRoles()
        {
            return await this.admin.UsersWithRolesAsync();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(nameof(Roles))]
        public async Task<string[]> Roles()
        {
            return await this.admin.RolesAsync();
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpGet(nameof(AdsForApproval))]
        public async Task<IEnumerable<AdForListDTO>> AdsForApproval()
        {
            return await this.admin.AdsForApprovalAsync();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut(nameof(EditUserRoles) + PathSeparator + "{userName}")]
        public async Task<ActionResult> EditUserRoles(string userName, RoleEditDTO model)
        {
            return await this.admin
                .EditUserRolesAsync(userName, model.RoleNames)
                .ToActionResult();
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpPut(nameof(ApproveAd) + PathSeparator + Id)]
        public async Task<ActionResult> ApproveAd(int id) 
        {
            return await this.admin
                .ApproveAdAsync(id)
                .ToActionResult();
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpDelete(nameof(RejectAd) + PathSeparator + Id)]
        public async Task<ActionResult> RejectAd(int id, [FromServices]IAdService ads) 
        {
            return await ads
                .DeleteAsync(id)
                .ToActionResult();
        }
    }
}
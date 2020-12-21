using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.DTOs.Role;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)   
        {
            _adminService = adminService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _adminService.GetUsersWithRolesAsync();
            
            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _adminService.GetRolesAsync();

            return Ok(roles);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("editRoles/{userName}")]
        public async Task<IActionResult> EditUserRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var result = await _adminService.EditUserRolesAsync(userName, roleEditDTO.RoleNames);

            if (result.Failure) 
            {
                return BadRequest(result.Error);
            }
            
            return Ok();
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpGet("adsForApproval")]
        public async Task<IActionResult> GetAdsForApproval()
        {
            var ads = await _adminService.GetAdsForApprovalAsync();

            return Ok(ads);
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpPut("approveAd/{id}")]
        public async Task<IActionResult> ApproveAd(int id) 
        {
            var result = await _adminService.ApproveAdAsync(id);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [Authorize(Policy = "RequireAdminOrModeratorRole")]
        [HttpDelete("rejectAd/{id}")]
        public async Task<IActionResult> RejectAd(int id, [FromServices]IAdService adService) 
        {
            var result = await adService.DeleteAsync(id);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
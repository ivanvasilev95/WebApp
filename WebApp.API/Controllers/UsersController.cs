using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.User;
using WebApp.API.Extensions;

namespace WebApp.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            bool includeNotApprovedAds = IsUserEligible(id);
            var result = await _userService.ByIdAsync(id, includeNotApprovedAds);
            if(result.Failure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Data);
        }

        private bool IsUserEligible(int userId) // move to user service
        {
            var loggedInUserId = int.Parse(this.User.GetId() ?? "0");
            if (this.User.Identity.IsAuthenticated) {
                var loggedInUserRoles = ((ClaimsIdentity)this.User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                return (loggedInUserId == userId || loggedInUserRoles.Contains("Admin") || loggedInUserRoles.Contains("Moderator"));
            }
            return false;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            var loggedInUserId = int.Parse(this.User.GetId());
            if (id != loggedInUserId)
                return Unauthorized();

            var result = await _userService.UpdateAsync(id, userForUpdateDTO);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }
    }
}
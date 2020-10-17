using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserWithAds(int id)
        {
            var result = await _userService.GetUserWithAdsAsync(id, this.User);
            if(result.Failure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> GetUserForEdit(int id)
        {
            var user = await _userService.GetUserForEditAsync(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            var result = await _userService.UpdateUserAsync(id, userForUpdateDTO);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
    }
}
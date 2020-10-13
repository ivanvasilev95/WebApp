using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.API.DTOs.User;
using WebApp.API.Data.Interfaces;

namespace WebApp.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(
            UserManager<User> userManager,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            if (await _userManager.FindByNameAsync(userForRegisterDTO.UserName) != null) 
            {
                return BadRequest("Вече има регистриран потребител с това потребителско име");
            }

            if (await _userManager.FindByEmailAsync(userForRegisterDTO.Email) != null) 
            {
                return BadRequest("Вече има регистриран потребител с този имейл адрес");
            }

            var userToCreate = _mapper.Map<User>(userForRegisterDTO);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDTO.Password);
            if (result.Succeeded) 
            {
                await _userManager.AddToRoleAsync(userToCreate, "Member");
                return Ok();
            }
            
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDTO.UserName);
            if (user == null)
            {
                return Unauthorized("Невалидено потребителско име или парола");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, userForLoginDTO.Password);
            if (passwordValid) 
            {
                return Ok(new
                {
                    token = _authService.GenerateJwtToken(user).Result
                });
            }

            return Unauthorized("Невалидено потребителско име или парола");  
        }
    }
}
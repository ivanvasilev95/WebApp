using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using WebApp.API.DTOs;
using WebApp.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApp.API.Controllers
{   
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDTO);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDTO.Password);

            var userToReturn = _mapper.Map<UserForDetailedDTO>(userToCreate);

            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(userToCreate, "Member");
                
                return CreatedAtRoute("GetUser",
                    new {controller = "Users", id = userToCreate.Id}, userToReturn);
            }
            
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDTO.Username);

            if (user == null)
                return Unauthorized("Невалидено потребителско име или парола");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDTO.Password, false);

            if (result.Succeeded) {
                // var appUser = await _userManager.Users//.Include(u => u.Ads)
                    // .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDTO.Username.ToUpper());
                return Ok(new
                {
                    token = GenerateJwtToken(user).Result
                });
            }

            return Unauthorized("Невалидено потребителско име или парола");  
        }

        private async Task<string> GenerateJwtToken(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebBazar.API.DTOs.User;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authentication;

        public AuthController(IAuthService authentication)
        {
            this.authentication = authentication;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginServiceModel>> Login(UserForLoginDTO model)
        {
            return await this.authentication
                .LoginAsync(model)
                .ToActionResult(); 
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult> Register(UserForRegisterDTO model)
        {   
            return await this.authentication
                .RegisterAsync(model)
                .ToActionResult();
        }
    }
}
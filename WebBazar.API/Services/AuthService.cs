using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebBazar.API.Data.Models;
using WebBazar.API.DTOs.User;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class AuthService : IAuthService
    {
        private const string InvalidErrorMessage = "Невалидено потребителско име или парола";

        private readonly UserManager<User> userManager;
        private readonly IJwtGeneratorService jwtGenerator;
        private readonly IMapper mapper;

        public AuthService(
            UserManager<User> userManager,
            IMapper mapper,
            IJwtGeneratorService jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.mapper = mapper;
        }

        public async Task<Result<LoginServiceModel>> LoginAsync(UserForLoginDTO model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return InvalidErrorMessage;
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid) 
            {
                return InvalidErrorMessage;
            }

            var token = await this.jwtGenerator.GenerateJwtTokenAsync(user);

            return new LoginServiceModel { Token = token };
        }

        public async Task<Result> RegisterAsync(UserForRegisterDTO model)
        {
            if (await this.userManager.FindByNameAsync(model.UserName) != null) 
            {
                return "Вече има регистриран потребител с това потребителско име";
            }

            if (await this.userManager.FindByEmailAsync(model.Email) != null) 
            {
                return "Вече има регистриран потребител с този имейл адрес";
            }

            var user = this.mapper.Map<User>(model);

            var identityResult = await this.userManager.CreateAsync(user, model.Password);
            
            if (!identityResult.Succeeded) 
            {
                return String.Join("\n", identityResult.Errors.Select(e => e.Description));
            }
            
            await this.userManager.AddToRoleAsync(user, "Member");

            return true;
        }
    }
}
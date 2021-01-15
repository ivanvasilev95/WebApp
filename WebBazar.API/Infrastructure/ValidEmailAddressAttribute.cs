using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using WebBazar.API.Data;

namespace WebBazar.API.Infrastructure
{
    public class ValidEmailAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const int mainAdminId = 1;

            var email = value?.ToString();

            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var request = httpContextAccessor.HttpContext.Request; 
            var userId = int.Parse(request.RouteValues["id"].ToString());
            
            if (userId != mainAdminId && string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult("Полето \'Имейл адрес\' не може да бъде празно");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!EmailIsValid(email))
                {
                    return new ValidationResult("Имейл адресът не е валиден");
                }
                
                if (EmailIsNotAvailable(userId, email, validationContext))
                {
                    return new ValidationResult("Вече има регистриран потребител с този имейл адрес");
                }
            }

            return ValidationResult.Success;
        }

        private bool EmailIsValid(string input)
        {
            try
            {
                var email = new MailAddress(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool EmailIsNotAvailable(int userId, string email, ValidationContext validationContext)
        {
            var dataContext = (DataContext)validationContext.GetService(typeof(DataContext));
            return dataContext.Users.Any(u => u.Id != userId && u.Email == email);
        }
    }
}
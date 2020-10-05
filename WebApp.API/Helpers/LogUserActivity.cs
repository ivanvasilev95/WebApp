using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.API.Data.Interfaces;
using WebApp.API.Extensions;

namespace WebApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var repo = (IUserRepository)resultContext.HttpContext.RequestServices.GetService(typeof(IUserRepository));
            var userId = int.Parse(resultContext.HttpContext.User.GetId() ?? "0");
            var user = await repo.GetUser(userId, false);
            if(user != null) {
                user.LastActive = DateTime.Now;
                await repo.SaveAll();
            }
        }
    }
}
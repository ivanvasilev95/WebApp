using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.API.Data;

namespace WebApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var userId = int.Parse(resultContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            var repo = (IUserRepository)resultContext.HttpContext.RequestServices.GetService(typeof(IUserRepository));
            var user = await repo.GetUser(userId, true);
            if(user != null)
                user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}
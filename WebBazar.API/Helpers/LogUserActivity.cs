using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data;
using WebApp.API.Extensions;

namespace WebApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            
            var dataContext = (DataContext)resultContext.HttpContext.RequestServices.GetService(typeof(DataContext));
            
            var userId = int.Parse(resultContext.HttpContext.User.GetId() ?? "0");

            var user = await dataContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.LastActive = DateTime.Now;
                await dataContext.SaveChangesAsync();
            }
        }
    }
}
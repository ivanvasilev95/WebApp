using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Infrastructure.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            
            var userId = int.Parse(resultContext.HttpContext?.User?.GetId() ?? "0");
            
            var data = (DataContext)resultContext.HttpContext.RequestServices.GetService(typeof(DataContext));

            var user = await data.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.LastActive = DateTime.Now;
                await data.SaveChangesAsync();
            }
        }
    }
}
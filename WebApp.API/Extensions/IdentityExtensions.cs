using System.Linq;
using System.Security.Claims;

namespace WebApp.API.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user
                ?.Claims
                ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        
        public static string GetUserName(this ClaimsPrincipal user)
            => user
                ?.Identity
                ?.Name;
                
        public static bool IsAuthenticated(this ClaimsPrincipal user)
            => user
                .Identity
                .IsAuthenticated;
    }
}
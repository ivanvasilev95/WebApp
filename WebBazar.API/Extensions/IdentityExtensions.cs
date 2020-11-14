using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApp.API.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        
        public static IEnumerable<string> GetRoles(this ClaimsPrincipal user) 
            => ((ClaimsIdentity)user.Identity)
                    .Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

        public static bool IsAuthenticated(this ClaimsPrincipal user)
            => user
                .Identity
                .IsAuthenticated;
    }
}
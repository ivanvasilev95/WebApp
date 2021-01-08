using Microsoft.Extensions.Configuration;

namespace WebBazar.API.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DbConnection");
    }
}
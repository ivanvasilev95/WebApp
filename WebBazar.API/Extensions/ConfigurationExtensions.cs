using Microsoft.Extensions.Configuration;

namespace WebBazar.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DbConnection");
    }
}
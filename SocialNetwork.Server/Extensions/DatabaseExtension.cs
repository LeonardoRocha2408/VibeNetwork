using Microsoft.EntityFrameworkCore;
using SocialNetwork.Server.Domain.Entities;
using SocialNetwork.Server.Infrastructure.Persistence;

namespace SocialNetwork.Server.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection DatabaseConfig(this IServiceCollection service)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new ArgumentNullException("The connection string can't be empty");

            service
                .AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );

            return service;
        }
    }
}

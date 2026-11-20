using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Domain.Entities;
using SocialNetwork.Server.Infrastructure.Services;
using SocialNetwork.Server.Services;

namespace SocialNetwork.Server.Extensions;

public static class AddServicesExtensions
{
    public static IServiceCollection AddServicesConfig(this IServiceCollection services)
    {
        services
            .AddScoped<RedisServices>()
            .AddScoped<PasswordHashService>()
            .AddSingleton<PasswordHasher<UserScaffold>>();
        
        return services;
    }
}
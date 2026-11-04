namespace SocialNetwork.Server.Extensions;

public static class CorsCustomExtension
{
    public static IServiceCollection CorsConfig(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") 
                ?? throw new Exception("FRONTEND_URL environment variable is not set.");
            options.AddPolicy("FrontendOnly", policy =>
            {
                policy.WithOrigins(frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });
        return service;
    }
}
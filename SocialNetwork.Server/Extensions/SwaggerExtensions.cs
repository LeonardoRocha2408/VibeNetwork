using Microsoft.OpenApi;
namespace SocialNetwork.Server.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGenConfig(this IServiceCollection service)
    {
        service
            .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Minha API", Version = "v1"

                    });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme."
                    });
                    c.AddSecurityRequirement(document => 
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                                new List<string>()
                        });
                }
            );
        return service;
    }
}
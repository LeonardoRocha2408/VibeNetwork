using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SocialNetwork.Server.Extensions;

public static class JwtConfigExtensions
{
    public static IServiceCollection AddJwtConfig(this IServiceCollection service)
    {
        var vIssuer = Environment.GetEnvironmentVariable("VALIDATE_ISSUER");
        if (!bool.TryParse(vIssuer, out var validateIssuer)) 
            throw new Exception("Invalid ISSUER");
        var vAudience = Environment.GetEnvironmentVariable("VALIDATE_AUDIENCE");
        if (!bool.TryParse(vAudience, out var validateAudience)) 
            throw new Exception("Invalid AUDIENCE");
        var vLifetime = Environment.GetEnvironmentVariable("VALIDATE_LIFETIME");
        if (!bool.TryParse(vLifetime, out var validateLifetime))
            throw new Exception("Invalid LIFETIME");
        var vIssuerSigningKey = Environment.GetEnvironmentVariable("VALIDATE_ISSUER_SIGNING_KEY");
        if (!bool.TryParse(vIssuerSigningKey, out var validIssuerSigningKey))
            throw new Exception("Invalid ISSUER_SIGNING_KEY");
        
        var validIssuer = Environment.GetEnvironmentVariable("ISSUER")
            ?? throw new Exception("Invalid ISSUER");
        var validAudience = Environment.GetEnvironmentVariable("AUDIENCE")
            ?? throw new Exception("Invalid AUDIENCE");
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
            ?? throw new Exception("Invalid SECRET_KEY");
        
        service
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = validateIssuer,
                    ValidIssuer = validIssuer,
                    
                    ValidateAudience = validateAudience,
                    ValidAudience = validAudience,
                    
                    ValidateLifetime = validateLifetime,
                    ValidateIssuerSigningKey = validIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey)),
                    ValidAlgorithms = new[] {SecurityAlgorithms.HmacSha256},
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
        return service;
    }
}
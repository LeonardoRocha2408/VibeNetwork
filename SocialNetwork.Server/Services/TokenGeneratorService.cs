using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SocialNetwork.Server.Application.Users.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;

namespace SocialNetwork.Server.Infrastructure.Services;
public static class TokenGeneratorService
{
    public static (string jwt, DateTime expiresAt) TokenGenerator(UserDto user)
    {
        const int AccesTokenMinutes = 30;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keyValue = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        if (keyValue == null) 
            throw new CredentialsNullException(Errors.InvalidCredentials, "Key can't be empty", null!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(AccesTokenMinutes);

        var token = new JwtSecurityToken
        (
            issuer: Environment.GetEnvironmentVariable("ISSUER"),
            audience: Environment.GetEnvironmentVariable("AUDIENCE"),
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler()
        .WriteToken(token);            
        return (jwt, expiresAt);
    }
}
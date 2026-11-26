using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Application.Users.DTOs;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using SocialNetwork.Server.Infrastructure.Persistence;
using SocialNetwork.Server.Infrastructure.Services;
using SocialNetwork.Server.Services;

namespace SocialNetwork.Server.Application.Users.Commands.Login;
public class LoginHandler(
    AppDbContext db, 
    PasswordHashService passwordHasher,
    IHttpContextAccessor context) : 
    IRequestHandler<LoginCommand, AuthResponseDto>
{

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var _context = context.HttpContext ?? throw new Exception("An unexpected error occurred");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, ct);

        if (user is null || passwordHasher.VerifyPassword(user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            throw new UnauthorizedException(Errors.UserNotFound, "Account not found");
        
        if (user.Role == UserRole.Banned) 
            throw new UnauthorizedException(Errors.UserBanned, "Your account has banned");

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            DisplayName = user.DisplayName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Role = user.Role
        };

        var raw = _context.Request.Cookies["refreshToken"];

        if (!string.IsNullOrWhiteSpace(raw))
        {    
            var hash = RefreshTokenService.Hash(raw);
            var token = await db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, ct);
            if (token is not null) token.RevokedAt = DateTime.UtcNow;
        }
        var (newRaw, newHash) = RefreshTokenService.Generate();

        var newToken = new RefreshTokenScaffold
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newHash,
        };
        await db.RefreshTokens.AddAsync(newToken, ct);
        await db.SaveChangesAsync(ct);

        _context.Response.Cookies.Append("refreshToken", newRaw, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        });

        var (jwt, expiresAt) = TokenGeneratorService.TokenGenerator(userDto);
        return new AuthResponseDto
        {
            User = userDto,
            Session = new SessionDto{ AccessToken = jwt, ExpireAt = expiresAt }                
        };
    }
}
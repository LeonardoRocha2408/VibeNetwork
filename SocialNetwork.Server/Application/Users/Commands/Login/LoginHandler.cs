using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Application.Users.DTOs;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using SocialNetwork.Server.Infrastructure.Persistence;
using SocialNetwork.Server.Infrastructure.Services;

namespace SocialNetwork.Server.Application.Users.Commands.Login;
public class LoginHandler(
    AppDbContext db, 
    PasswordHashService passwordHasher,
    IHttpContextAccessor context) : 
    IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly AppDbContext _db = db;
    private readonly PasswordHashService _passwordHasher = passwordHasher;

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var _context = context.HttpContext ?? throw new Exception("An unexpected error occurred");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || _passwordHasher.VerifyPassword(user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            throw new UnauthorizedException(Errors.UserNotFound, "Account not found");
        
        if (user.Role == UserRole.Banned) 
            throw new UnauthorizedException(Errors.UserBanned, "Your account has banned");

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            DisplayName = user.DisplayName,
            ProfilePictureUrl = user.ProfilePictureURL,
            Role = user.Role
        };

        var raw = _context.Request.Cookies["refreshToken"];

        if (!string.IsNullOrWhiteSpace(raw))
        {    
            var hash = RefreshTokenService.Hash(raw);
            var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash);
            if (token is not null) token.RevokedAt = DateTime.UtcNow;
        }
        var (newRaw, newHash) = RefreshTokenService.Generate();

        var newToken = new RefreshTokenScaffold
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newHash,
        };
        await _db.RefreshTokens.AddAsync(newToken);
        await _db.SaveChangesAsync();

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
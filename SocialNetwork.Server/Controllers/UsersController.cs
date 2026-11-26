using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Server.Application.Users.Commands.CreateUser;
using SocialNetwork.Server.Application.Users.Commands.Login;
using SocialNetwork.Server.Application.Users.DTOs;
using SocialNetwork.Server.Services;

namespace SocialNetwork.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(ISender sender) : ControllerBase
    {
        
        // ===============
        // Post endpoints
        // ===============
        
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(
            CreateUserCommand user, 
            CancellationToken ct = default)
        {
            var id = await sender.Send(user, ct);
            return Created($"api/user/{id}", new { id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount(
            LoginCommand user, 
            CancellationToken ct = default)
        {
            var sessionData = await sender.Send(user, ct);
            return Ok(sessionData);
        }
        
        [HttpPost("refresh-token")]
        [Authorize]
        [ValidateToken("access")]
        public async Task<IActionResult> RefreshToken(
            UserDto user,
            HttpContext context, 
            CancellationToken ct = default)
        {
            var token = TokenGeneratorService.TokenGenerator(user);
            return Ok(token);
        }
    }
}

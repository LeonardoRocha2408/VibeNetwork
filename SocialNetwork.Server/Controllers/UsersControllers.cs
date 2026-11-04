using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Server.Application.Users.Commands.CreateUser;
using SocialNetwork.Server.Application.Users.Commands.Login;

namespace SocialNetwork.Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(ISender sender) : ControllerBase
    {
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserCommand user, CancellationToken ct)
        {
            var id = await sender.Send(user, ct);
            return Created($"api/user/{id}", new { id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount(LoginCommand user, CancellationToken ct)
        {
            var sessionData = await sender.Send(user, ct);
            return Ok(sessionData);
        }
    }
}

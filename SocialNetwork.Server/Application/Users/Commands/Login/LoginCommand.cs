using MediatR;
using SocialNetwork.Server.Application.Users.DTOs;

namespace SocialNetwork.Server.Application.Users.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password
    ) : IRequest<AuthResponseDto>;
}
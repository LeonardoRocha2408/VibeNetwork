using MediatR;
using SocialNetwork.Server.Application.Users.DTOs;

namespace SocialNetwork.Server.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string Email,
        string Username,
        string Password
        ) : IRequest<Guid>;
}

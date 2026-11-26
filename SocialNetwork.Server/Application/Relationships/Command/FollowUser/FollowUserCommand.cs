using MediatR;

namespace SocialNetwork.Server.Application.Relationships.Command.FollowUser;

public sealed record FollowUserCommand(
    Guid FollowedUserId,
    Guid UserId,
    bool IsFollowing
) : IRequest<bool>;
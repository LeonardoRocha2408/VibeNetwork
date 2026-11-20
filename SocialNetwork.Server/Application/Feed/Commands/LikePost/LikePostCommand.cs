using MediatR;

namespace SocialNetwork.Server.Application.Feed.Commands.LikePost;

public sealed record LikePostCommand(
    Guid PostId,
    Guid UserId,
    bool IsLiked
    ) : IRequest<bool>;
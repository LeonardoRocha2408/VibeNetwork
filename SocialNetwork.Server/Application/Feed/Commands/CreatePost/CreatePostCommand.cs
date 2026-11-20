using MediatR;
using SocialNetwork.Server.DTOs;

public sealed record CreatePostCommand(
    Guid UserId,
    string UserDisplayName,
    string Content,
    IFormFile? Media,
    int FollowersCount,
    PostVisibility Visibility
) : IRequest<PostDto>;
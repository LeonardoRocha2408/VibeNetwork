using MediatR;
using SocialNetwork.Server.DTOs;

public sealed record GetFeedQuery(
    Guid UserId,
    int FollowedsCount, 
    int Page,
    int PageSize = 20
) : IRequest<IReadOnlyList<PostDto>>;
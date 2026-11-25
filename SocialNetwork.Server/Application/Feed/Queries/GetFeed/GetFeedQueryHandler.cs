
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using SocialNetwork.Server.DTOs;
using SocialNetwork.Server.Infrastructure.Persistence;
using SocialNetwork.Server.Services;
using StackExchange.Redis;

namespace SocialNetwork.Server.Application.Feed.Queries;

public class GetFeedQueryHandler(
    AppDbContext db,
    IDriver neoDatabase,
    RedisServices redis) : IRequestHandler<GetFeedQuery, IReadOnlyList<PostDto>>
{
    public async Task<IReadOnlyList<PostDto>> Handle(GetFeedQuery request, CancellationToken ct)
    {
        await using var session = neoDatabase.AsyncSession();

        var likedResult = await session.RunAsync(
            """
            MATCH (u:User {id: $userId})-[:LIKED]->(p:Post)
            RETURN p.id AS PostId
            """,
            new
            {
                userId = request.UserId.ToString()
            });

        var likedPosts = (await likedResult.ToListAsync(
            record => Guid.Parse(record["PostId"].As<string>())
        )).ToHashSet();

        // Busca os usuários que o usuário atual segue
        var result = await session.RunAsync(
            """
            MATCH (u:User {id: $userId})-[:FOLLOWS]->(f:User)
            RETURN f.id AS FollowersId
            """,
            new
            {
                userId = request.UserId.ToString()
            });

        var followersList = await result.ToListAsync(
            f => f["FollowersId"].As<string>());

        var database = redis.GetDatabase();

        RedisKey feedKey = $"feed:user:{request.UserId}";

        var postsId = await database.SortedSetRangeByRankAsync(
            feedKey,
            (request.Page - 1) * request.PageSize,
            request.PageSize,
            Order.Descending
        );

        var postGuids = postsId.Select(p => Guid.Parse(p.ToString()));

        var normalPosts = await db.Posts
            .AsNoTracking()
            .Where(p => postGuids.Contains(p.Id))
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostDto
            {
                Id = p.Id,
                UserDisplayName = p.Author.DisplayName,
                Content = p.Content,
                CommentsCount = p.CommentsCount,
                LikesCount = p.LikesCount
            })
            .ToListAsync(ct);

        var popularIds = await db.Users
            .AsNoTracking()
            .Where(u => followersList.Contains(u.Id.ToString()))
            .Select(u => u.Id)
            .ToArrayAsync(ct);

        var batch = database.CreateBatch();
        var tasks = new List<Task<RedisValue[]>>();

        foreach (var popularId in popularIds)
        {
            RedisKey popularPost = $"post:user:{popularId}";

            tasks.Add(batch.SortedSetRangeByRankAsync(
                popularPost,
                (request.Page - 1) * request.PageSize,
                request.PageSize,
                Order.Descending));
        }

        batch.Execute();

        var resultTasks = await Task.WhenAll(tasks);

        var popularPosts = resultTasks
            .SelectMany(p => p)
            .Select(p => JsonSerializer.Deserialize<PostDto>(p.ToString()))
            .OfType<PostDto>()
            .ToList();

        var allPosts = popularPosts.Concat(normalPosts).Distinct().ToList();

        // Marca quais posts foram curtidos pelo usuário
        foreach (var post in allPosts)
        {
            post.AlreadyLiked = likedPosts.Contains(post.Id);
        }

        // Fallback: busca posts diretamente no banco
        if (request.FollowedsCount == 0 || allPosts.Count == 0)
        {
            var posts = await db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    UserDisplayName = p.Author.DisplayName,
                    Content = p.Content,
                    CommentsCount = p.CommentsCount,
                    LikesCount = p.LikesCount
                })
                .ToListAsync(ct);

            foreach (var post in posts)
            {
                post.AlreadyLiked = likedPosts.Contains(post.Id);
            }

            return posts;
        }

        return allPosts;
    }
}

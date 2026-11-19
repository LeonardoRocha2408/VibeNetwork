using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using SocialNetwork.Server.DTOs;
using SocialNetwork.Server.Infrastructure.Persistence;
using StackExchange.Redis;

namespace  SocialNetwork.Server.Application.Feed.Queries;
public class GetFeedQueryHandler(
    AppDbContext db, 
    IDriver neoDatabase,
    RedisServices redis) : IRequestHandler<GetFeedQuery, IReadOnlyList<PostDto>>
{
    public async Task<IReadOnlyList<PostDto>> Handle(GetFeedQuery request, CancellationToken ct)
    {
        await using var session = neoDatabase.AsyncSession();
        var result = await session.RunAsync(
            """
            MATCH (u:User {id: $userId})-[:FOLLOWS]->(f:User)
            RETURN f AS FollowersId
            """,
            new
            {
                userId = request.UserId
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
        if (request.FollowedsCount == 0 || allPosts.Count == 0)
        {
            var posts = await db.Posts
              //  .Where(p => p.Author.FollowersCount == 100000)
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
            return posts;
        }
        
        return allPosts;
    }
}

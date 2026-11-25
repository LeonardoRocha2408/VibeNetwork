using MediatR;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using SocialNetwork.Server.Infrastructure.Persistence;
namespace SocialNetwork.Server.Application.Feed.Commands.LikePost;

public class LikePostHandler(IDriver neo, AppDbContext db) : IRequestHandler<LikePostCommand, bool>
{
    
    public async Task<bool> Handle(LikePostCommand request, CancellationToken ct)
    {
        var session = neo.AsyncSession();

        if (request.IsLiked)
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                var result = await tx.RunAsync(
                    """
                    MATCH (u:User {id: $userId})-[r:LIKED]->(p:Post {id: $postId})
                    DELETE r
                    """,
                    new
                    {
                        userId = request.UserId.ToString(),
                        postId = request.PostId.ToString()
                    }
                );
                await result.ConsumeAsync();
            });

            await db.Posts
                .Where(p => p.Id == request.PostId)
                .ExecuteUpdateAsync(s => s 
                .SetProperty(p => p.LikesCount, p => p.LikesCount - 1), ct);

            return true;
        }
        await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(
                """
                MATCH (u:User {id: $userId})
                MATCH (p:Post {id: $postId})
                CREATE (u)-[:LIKED]->(p)
                """, new
                {
                    userId = request.UserId.ToString(),
                    postId = request.PostId.ToString()
                });
            await result.ConsumeAsync();
        });

        await db.Posts
                .Where(p => p.Id == request.PostId)
                .ExecuteUpdateAsync(s => s 
                .SetProperty(p => p.LikesCount, p => p.LikesCount + 1), ct);

        return true;
    }
}
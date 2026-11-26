using MediatR;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using SocialNetwork.Server.Infrastructure.Persistence;

namespace SocialNetwork.Server.Application.Relationships.Command.FollowUser;

public class FollowUserHandler(IDriver neoDatabase) : IRequestHandler<FollowUserCommand, bool>
{
    public async Task<bool> Handle(FollowUserCommand request, CancellationToken ct)
    {
        using var session = neoDatabase.AsyncSession();
        
        if (request.IsFollowing)
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                var result = await tx.RunAsync(
                    """
                    MATCH (u:User {id: $userId})-[r:FOLLOWS]->(f:User {id: $followedUserId})
                    DELETE r
                    """,
                    new
                    {
                        userId = request.UserId.ToString(),
                        followedUserId = request.FollowedUserId.ToString()
                    });
                await result.ConsumeAsync();
            });
            return true;
        }
        await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(
                """
                MATCH (u:User {id: $userId})
                MATCH (f:User {id: $followedUserId})
                CREATE (u)-[r:FOLLOWS]->(f)
                """,
                new
                {
                    userId = request.UserId.ToString(),
                    followedUserId = request.FollowedUserId.ToString()
                });
            await result.ConsumeAsync();
        });
        return true;
    }
}
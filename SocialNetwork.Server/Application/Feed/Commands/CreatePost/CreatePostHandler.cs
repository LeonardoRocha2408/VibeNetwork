using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using MediatR;
using Neo4j.Driver;
using SocialNetwork.Server.DTOs;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using SocialNetwork.Server.Infrastructure.Persistence;

namespace SocialNetwork.Server.Application.Feed.Commands;

public class CreatePostHandler(
    AppDbContext db,
    IDriver neoDatabase,
    IAmazonSQS sqs
    ) : IRequestHandler<CreatePostCommand, PostDto>
{
    private readonly string _queueUrl = Environment.GetEnvironmentVariable("QUEUE_URL")
        ?? throw new CredentialsNullException(Errors.InvalidCredentials, "Credentials cannot be empty.");
    
    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken ct)
    {
        var newPost = new PostScaffold(request.UserId, request.Content, request.Visibility);
        await db.Posts.AddAsync(newPost, ct);
        await db.SaveChangesAsync(ct);

        await using var session = neoDatabase.AsyncSession();
        await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(
                """
                MATCH (u:User {id: $userId})
                CREATE (p:Post {id: $postId})
                CREATE (u)-[r:CREATED]->(p)
                """,
                new
                {
                    userId = request.UserId.ToString(),
                    postId = newPost.Id.ToString()
                }
            );
            await result.ConsumeAsync();
        });
        
        var json = JsonSerializer.Serialize(new
        {
            owner_id = request.UserId,
            user_name = request.UserDisplayName,
            followed_count = request.FollowersCount,
            content = request.Content,
            comments_count = newPost.CommentsCount,
            likes_count = newPost.LikesCount,
        });
        
        await sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = json
        }, ct);
        
        return new PostDto
        {
            Id = newPost.Id,
            UserDisplayName = request.UserDisplayName,
            Content = newPost.Content,
            CommentsCount = 0,
            LikesCount = 0,
        };
    }
}
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SocialNetwork.Server.Services;

public interface ISqsService
{
    Task SendMessage<T>(T message, string queueUrl, CancellationToken ct);
}

public sealed class SqsService(IAmazonSQS sqs) : ISqsService
{
    public async Task SendMessage<T>(T message, string queueUrl, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(message);
        
        await sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = json
        }, ct);
    }
}
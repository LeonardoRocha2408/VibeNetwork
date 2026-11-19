using Amazon.Runtime;
using Amazon.SQS;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;

public static class AmazonSqsExtensions
{
    public static IServiceCollection AmazonSqsConfig(this IServiceCollection service)
    {
        var region = Environment.GetEnvironmentVariable("AWS_REGION")
            ?? throw new CredentialsNullException(Errors.InvalidCredentials, "Credentials cannot be empty.");

        var credentials = new BasicAWSCredentials(
            Environment.GetEnvironmentVariable("ACCESS_KEY")
                ?? throw new CredentialsNullException(Errors.InvalidCredentials, "Credentials cannot be empty."),
            Environment.GetEnvironmentVariable("ACCESS_SECRET_KEY")
                ?? throw new CredentialsNullException(Errors.InvalidCredentials, "Credentials cannot be empty.")
        );

        var config = new AmazonSQSConfig
        {
            AuthenticationRegion = Environment.GetEnvironmentVariable("AWS_REGION")
        };

        service
            .AddSingleton<IAmazonSQS>(
                new AmazonSQSClient(credentials, config)
            );
        return service;
    }
}
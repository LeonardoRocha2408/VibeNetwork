using Neo4j.Driver;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;

namespace SocialNetwork.Server.Extensions;

public static class NeoDatabaseExtension
{
    public static IServiceCollection AddNeoDatabaseConfig(this IServiceCollection services)
    {
        var uri = Environment.GetEnvironmentVariable("NEO4J_URI")
                            ?? throw new CredentialsNullException(Errors.InvalidCredentials,
                                "Credentials cannot be empty");
        var username = Environment.GetEnvironmentVariable("NEO4J_USERNAME")
                       ?? throw new CredentialsNullException(Errors.InvalidCredentials,
                           "Credentials cannot be empty");
        var password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD")
                       ?? throw new CredentialsNullException(Errors.InvalidCredentials,
                           "Credentials cannot be empty");
        
        return services.AddSingleton<IDriver>(
            GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)));
    }
}
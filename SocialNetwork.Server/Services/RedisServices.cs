using SocialNetwork.Server.Infrastructure.Common.Exceptions;
using StackExchange.Redis;

public class RedisServices
{
    private readonly ConnectionMultiplexer _redis;
    public RedisServices()
    {
        string connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
            ?? throw new CredentialsNullException(Errors.InvalidCredentials, "Credencials is not empty");
        
        _redis = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase GetDatabase()
    {
        return _redis.GetDatabase();
    }
}
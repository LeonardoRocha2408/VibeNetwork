using SocialNetwork.Server.Application;

public static class MediatrExtension
{
    public static IServiceCollection MediatrConfig(this IServiceCollection service)
    {
        service
        .AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        }
        );
        return service;
    }
}
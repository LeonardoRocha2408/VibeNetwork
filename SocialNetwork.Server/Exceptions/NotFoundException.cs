namespace SocialNetwork.Server.Infrastructure.Common.Exceptions
{
    public class NotFoundException(Errors code, string message, params object?[] args) : Exception(string.Format(message, args))
    {
        public Errors Code {get;} = code;
    }
}

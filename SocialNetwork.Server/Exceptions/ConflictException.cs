namespace SocialNetwork.Server.Infrastructure.Common.Exceptions
{
    public class ConflictException(Errors code, string message, params object?[] args) : SystemException(string.Format(message, args))
    {
        public Errors Code {get;} = code;
    }
}
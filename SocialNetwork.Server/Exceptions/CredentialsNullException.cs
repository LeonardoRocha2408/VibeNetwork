namespace SocialNetwork.Server.Infrastructure.Common.Exceptions 
{
    public class CredentialsNullException(Errors code, string message, params object?[] args) : Exception(string.Format(message, args))
    {
        public Errors Code {get;} = code;
    }
}
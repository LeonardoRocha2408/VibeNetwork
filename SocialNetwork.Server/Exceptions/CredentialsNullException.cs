namespace SocialNetwork.Server.Infrastructure.Common.Exceptions;

public class CredentialsNullException(Errors code, string message, params object[]? args) 
    : ExceptionBase(code, message, args);
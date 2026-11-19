namespace SocialNetwork.Server.Infrastructure.Common.Exceptions;

public class NotFoundException(Errors code, string message, params object[]? args) 
    : ExceptionBase(code, message, args);

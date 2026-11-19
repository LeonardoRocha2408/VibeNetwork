namespace SocialNetwork.Server.Infrastructure.Common.Exceptions;

public class ConflictException(Errors code, string message, params object[]? args) 
    : ExceptionBase(code, message, args);
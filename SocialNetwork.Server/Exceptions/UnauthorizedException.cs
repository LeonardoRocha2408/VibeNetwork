public class UnauthorizedException(Errors code, string message, params object[]? args) 
    : ExceptionBase(code, message, args);
public class ForbiddenException(Errors code, string message, params object[]? args) 
    : ExceptionBase(code, message, args);
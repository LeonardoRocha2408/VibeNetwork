public class UnauthorizedException(Errors code, string message, params object?[] args) : SystemException(string.Format(message, args))
{
    public Errors Code { get; } = code;
}
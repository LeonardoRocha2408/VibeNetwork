using System.Net;
using SocialNetwork.Server.DTOs;
using SocialNetwork.Server.Infrastructure.Common.Exceptions;

namespace SocialNetwork.Server.Middlewares;
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleAsync(context, exception);
        }
    }

    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, code) = MapExceptionToResponse(exception);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(new ErrorResponse(false, (int)statusCode, message, code));
    }
    private static (HttpStatusCode statusCode, string message, string? code) MapExceptionToResponse(Exception exception)
    {
        return exception switch
        {
            ConflictException ex => (HttpStatusCode.Conflict, ex.Message, ex.Code.ToString()),
            CredentialsNullException ex => (HttpStatusCode.InternalServerError, ex.Message, ex.Code.ToString()),
            NotFoundException ex => (HttpStatusCode.NotFound, ex.Message, ex.Code.ToString()),
            UnauthorizedException ex => (HttpStatusCode.Unauthorized, ex.Message, ex.Code.ToString()),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred", null)
        };
    }
}
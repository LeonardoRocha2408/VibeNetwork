using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateToken(string tokenType) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if(user.Identity?.IsAuthenticated != true) 
            throw new UnauthorizedException(Errors.Unauthorized, "User is not authenticated");


        string token = user.FindFirst("token_type")?.Value 
            ?? throw new ForbiddenException(Errors.Forbidden, "Invalid token type");

        if (!token.Equals(tokenType, StringComparison.OrdinalIgnoreCase))
            throw new ForbiddenException(Errors.Forbidden, "Invalid token type");
    }
}

using System.Net;

public static class ErrorsStatus
{
    public static HttpStatusCode From(Errors errors)
    {
        return errors switch
        {
            Errors.EmailAlreadyExists or Errors.UsernameAlreadyExists => HttpStatusCode.Conflict,
            Errors.InvalidRefreshToken or Errors.SessionExpired => HttpStatusCode.Unauthorized,
            Errors.Forbidden => HttpStatusCode.Forbidden,
            Errors.Unauthorized => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
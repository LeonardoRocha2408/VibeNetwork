namespace SocialNetwork.Server.Application.Users.DTOs
{
    public class AuthResponseDto
    {
        public UserDto User {get; set;} = new();
        public SessionDto Session {get; set;} = new();
    }
    public class UserDto
    {
        public Guid Id {get; set;}
        public string Email {get; set;} = string.Empty;
        public string Username {get; set;} = string.Empty;
        public string DisplayName {get; set;} = string.Empty;
        public string ProfilePictureUrl {get; set;} = string.Empty;
        public UserRole Role {get; set;}
    }
    public class SessionDto
    {
        public string AccessToken {get; set;} = string.Empty;
        public DateTime ExpireAt {get; init;}
    }
}
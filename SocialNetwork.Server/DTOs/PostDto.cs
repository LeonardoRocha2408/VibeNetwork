namespace SocialNetwork.Server.DTOs;
public class PostDto
{
    public Guid Id { get; set; }
    public string UserDisplayName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool AlreadyLiked { get; set; }
    public int CommentsCount { get; set; }
    public int LikesCount { get; set; }
    public int PageSize { get; set; } = 20;
}
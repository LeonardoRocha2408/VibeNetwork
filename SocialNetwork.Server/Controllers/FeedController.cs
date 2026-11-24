using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Server.Application.Feed.Commands.LikePost;

[Route("api/feed")]
[ApiController]
public class FeedController(ISender sender) : ControllerBase
{
    // ===============
    // Post endpoints
    // ===============
    [HttpPost]
    [Route("create-post")]
    public async Task<IActionResult> CreatePost(CreatePostCommand post, CancellationToken ct = default)
    {
        var postData = await sender.Send(post, ct);
        return Ok(postData);
    }
    
    [HttpPost]
    [Route("get-feed")]
    public async Task<IActionResult> GetFeed([FromBody] GetFeedQuery request, CancellationToken ct = default)
    {
        var feed = await sender.Send(request, ct);
        return Ok(feed);
    }

    [HttpPost]
    [Route("like-post")]
    [ValidateToken("access")]
    [Authorize]
    public async Task<IActionResult> LikePost([FromBody] LikePostCommand request, CancellationToken ct = default)
    {
        var postLiked = await sender.Send(request, ct);
        return Ok(new { success = postLiked });
    }
}
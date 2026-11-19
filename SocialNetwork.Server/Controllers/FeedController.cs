using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/feed")]
[ApiController]
[ValidateToken("access")]
public class FeedController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Route("create-post")]
    public async Task<IActionResult> CreatePost(CreatePostCommand post, CancellationToken ct)
    {
        var postData = await sender.Send(post, ct);
        return Ok(postData);
    }
    
    // {userId:guid}/{followedsCount:int}/{page:int?}
    [HttpPost]
    [Route("get-feed")]
    public async Task<IActionResult> GetFeed([FromBody] GetFeedQuery request, CancellationToken ct)
    {
        var feed = await sender.Send(request, ct);
        return Ok(feed);
    }
}
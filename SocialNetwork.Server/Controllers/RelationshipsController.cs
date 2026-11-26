using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Server.Application.Relationships.Command.FollowUser;

[Authorize]
[Route("api/relationships")]
[ApiController]
public class RelationshipsController(ISender sender) : ControllerBase
{
    [Route("follow-user")]
    [HttpPost]
    public async Task<IActionResult> FollowerUser(FollowUserCommand follow, CancellationToken ct = default)
    {
        var result = await sender.Send(follow, ct);
        return Ok(result);
    }
}

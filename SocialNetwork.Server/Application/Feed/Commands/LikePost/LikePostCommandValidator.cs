using FluentValidation;

namespace SocialNetwork.Server.Application.Feed.Commands.LikePost;

public class LikePostCommandValidator : AbstractValidator<LikePostCommand>
{
    public LikePostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
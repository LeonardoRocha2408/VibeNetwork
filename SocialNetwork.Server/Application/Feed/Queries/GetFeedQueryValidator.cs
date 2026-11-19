using FluentValidation;

namespace SocialNetwork.Server.Application.Feed.Queries;

public sealed class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    public GetFeedQueryValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty");
    }
}
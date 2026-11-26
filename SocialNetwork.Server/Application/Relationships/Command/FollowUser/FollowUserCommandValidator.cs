using FluentValidation;

namespace SocialNetwork.Server.Application.Relationships.Command.FollowUser;

public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator()
    {
        RuleFor(f => f.FollowedUserId)
            .NotEmpty()
            .WithMessage("FollowedUserId should not be empty");
        
        RuleFor(f => f.UserId)
            .NotEmpty()
            .WithMessage("UserId should not be empty");
    }
}
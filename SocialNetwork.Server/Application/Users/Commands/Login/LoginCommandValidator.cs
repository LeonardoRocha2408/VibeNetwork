using FluentValidation;

namespace SocialNetwork.Server.Application.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Invalid email");
            
            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .Matches("[^a-zA-Z-0-9]").WithMessage("Password must have at least 8 characters")
                .Matches("[a-z]").WithMessage("The password must have at least one lowercase character")
                .Matches("[A-Z]").WithMessage("The password must have at least one uppercase character")
                .Matches("[0-9]").WithMessage("The password must have at least one numeric character");
        }
    }
}
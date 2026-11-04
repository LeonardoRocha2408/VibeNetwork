using FluentValidation;

namespace SocialNetwork.Server.Application.Users.Commands.CreateUser
{
    public class CreteUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreteUserCommandValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Invalid format")
                .MinimumLength(5);

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .MinimumLength(3).WithMessage("Username must have at least 3 characters")
                .MaximumLength(100).WithMessage("Username can't exceed 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .Matches("[^a-zA-Z0-9]").WithMessage("The password must have at least one special character")
                .Matches("[a-z]").WithMessage("The password must have at least one lowercase character")
                .Matches("[A-Z]").WithMessage("The password must have at least one uppercase character")
                .Matches("[0-9]").WithMessage("The password must have at least one numeric character");
        }
    }
}

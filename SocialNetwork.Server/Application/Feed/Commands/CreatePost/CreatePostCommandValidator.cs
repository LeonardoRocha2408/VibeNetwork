using FluentValidation;

public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly string[] allowedTypes = ["image/jpeg", "image/png", "image/jpeg", "image/webp"];
    private const long maxLength = 10 * 1024 * 1024;
    public CreatePostCommandValidator()
    {
        RuleFor(p => p.UserId)
        .NotEmpty().WithMessage("User ID cannot be empty");
        
        RuleFor(p => p.Content)
        .NotNull().WithMessage("Post content cannot be empty")
        .NotEmpty().WithMessage("Post content cannot be empty")
        .MaximumLength(255).WithMessage("Post has must have a maximum length of 255 characters");

        RuleFor(p => p.Media)
        .Must(m => m?.Length <= maxLength).WithMessage($"Image must be at most 10 MB")
        .Must(m => allowedTypes.Contains(m?.ContentType))
            .WithMessage("Image type is not allowed type")
            .When(p => p.Media is not null);

        RuleFor(p => p.Visibility)
        .IsInEnum().WithMessage("Invalid visibility value");
    }
}
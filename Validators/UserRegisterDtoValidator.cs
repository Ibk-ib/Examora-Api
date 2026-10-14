using FluentValidation;

public class UserRegisterDtoValidator
    : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(u => u.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(150)
            .WithMessage(
                "Full name cannot exceed 150 characters.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Enter a valid email address.")
            .MaximumLength(256)
            .WithMessage(
                "Email cannot exceed 256 characters.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(12)
            .WithMessage(
                "Password must contain at least 12 characters.")
            .MaximumLength(128)
            .WithMessage(
                "Password cannot exceed 128 characters.");
    }
}
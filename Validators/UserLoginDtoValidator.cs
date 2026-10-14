using FluentValidation;

public class UserLoginDtoValidator
    : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Enter a valid email address.")
            .MaximumLength(256);

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MaximumLength(128);
    }
}
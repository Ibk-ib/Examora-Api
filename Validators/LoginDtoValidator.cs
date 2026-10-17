using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.MatricNumber)
                .NotEmpty()
                .WithMessage("Matric number is required");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
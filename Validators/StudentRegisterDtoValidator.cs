
using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators
{
    public class StudentRegisterDtoValidator
        : AbstractValidator<StudentRegisterDto>
    {
        public StudentRegisterDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(254);

            RuleFor(x => x.MatricNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Department)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Level)
                .InclusiveBetween(100, 700);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(15);
        }
    }
}

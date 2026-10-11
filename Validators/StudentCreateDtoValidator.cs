using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator() { 
        RuleFor (student => student.FirstName)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");

        RuleFor (student => student.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");

        RuleFor (student => student.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(student => student.MatricNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Matric number is required.")
            .MaximumLength(30)
            .WithMessage("Matric number cannot exceed 30 characters.");

        RuleFor(student => student.Department)
           .Cascade(CascadeMode.Stop)
           .NotEmpty()
           .WithMessage("Department is required.")
           .MaximumLength(100)
           .WithMessage("Department cannot exceed 100 characters.");

        RuleFor(student => student.Level)
            .Must(level =>
                level >= 100 &&
                level <= 600 &&
                level % 100 == 0)
            .WithMessage(
                "Level must be 100, 200, 300, 400, 500 or 600.");
    }

    }


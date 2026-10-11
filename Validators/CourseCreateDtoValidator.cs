using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateDtoValidator() { 
        RuleFor (course => course.CourseCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage ("Course code is required.") 
            .MaximumLength(10)
            .WithMessage("Course code cannot exceed 20 characters.");

        RuleFor (course => course.CourseTitle)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Course title is required.")
            .MaximumLength(150)
            .WithMessage("Course title cannot exceed 150 characters.");

        RuleFor(course => course.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(course => course.CreditUnit)
            .InclusiveBetween(1, 6)
            .WithMessage("Credit unit must be between 1 and 6.");

        RuleFor(course => course.Department)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Department is required.")
            .MaximumLength(100)
            .WithMessage("Department cannot exceed 100 characters.");

        RuleFor(course => course.Level)
            .Must(level =>
                level >= 100 &&
                level <= 600 &&
                level % 100 == 0)
            .WithMessage(
                "Level must be 100, 200, 300, 400, 500 or 600.");


    } 
}


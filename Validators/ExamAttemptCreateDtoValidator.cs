
using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class ExamAttemptCreateDtoValidator
    : AbstractValidator<ExamAttemptCreateDto>
{
    public ExamAttemptCreateDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("Student ID is required.");

        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("Course ID is required.");
    }
}

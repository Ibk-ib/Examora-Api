
using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class QuestionCreateDtoValidator
    : AbstractValidator<QuestionCreateDto>
{
    public QuestionCreateDtoValidator()
    {
        RuleFor(q => q.CourseId)
            .NotEmpty()
            .WithMessage("Course ID is required.");

        RuleFor(q => q.QuestionText)
            .NotEmpty()
            .MaximumLength(1000)
            .WithMessage("Question text is required and must not exceed 1000 characters.");

        RuleFor(q => q.OptionA).NotEmpty();
        RuleFor(q => q.OptionB).NotEmpty();
        RuleFor(q => q.OptionC).NotEmpty();
        RuleFor(q => q.OptionD).NotEmpty();

        RuleFor(q => q.CorrectAnswer)
            .NotEmpty()
            .Must(a => new[] { "A", "B", "C", "D" }
                .Contains(a.Trim().ToUpperInvariant()))
            .WithMessage("Correct answer must be A, B, C or D.");

        RuleFor(q => q.Marks)
            .GreaterThan(0)
            .WithMessage("Marks must be greater than zero.");
    }
}

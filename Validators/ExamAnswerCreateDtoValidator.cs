
using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class ExamAnswerCreateDtoValidator
    : AbstractValidator<ExamAnswerCreateDto>
{
    public ExamAnswerCreateDtoValidator()
    {
        RuleFor(a => a.ExamAttemptId)
            .NotEmpty()
            .WithMessage("Exam attempt ID is required.");

        RuleFor(a => a.QuestionId)
            .NotEmpty()
            .WithMessage("Question ID is required.");

        RuleFor(a => a.SelectedAnswer)
            .NotEmpty()
            .Must(a => new[] { "A", "B", "C", "D" }
                .Contains(a.Trim().ToUpperInvariant()))
            .WithMessage("Selected answer must be A, B, C or D.");
    }
}

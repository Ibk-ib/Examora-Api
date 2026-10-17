
using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators;

public class ResultCreateDtoValidator
    : AbstractValidator<ResultCreateDto>
{
    public ResultCreateDtoValidator()
    {
        RuleFor(x => x.ExamAttemptId)
            .NotEmpty()
            .WithMessage("Exam attempt ID is required.");
    }
}

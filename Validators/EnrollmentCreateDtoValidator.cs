using FluentValidation;
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Validators
{
    public class EnrollmentCreateDtoValidator : AbstractValidator<EnrollmentCreateDto>
    {
        public EnrollmentCreateDtoValidator() { 
            
            RuleFor (e => e.StudentId)
                .NotEmpty()
                .WithMessage("Student ID is required.");  

            RuleFor (e => e.CourseId)
                .NotEmpty()
                .WithMessage("Course ID is required.");

        }
    }
}

using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;
using Microsoft.Extensions.Logging;

namespace UniversityAcademicApi.Services
{
    public class ExamAttemptService
        (IExamAttemptRepository examAttemptRepository, ILogger<ExamAttemptService> logger): IExamAttemptService
    {
        public async Task<ResponseModel<ExamAttemptDto>>
        CreateExamAttemptAsync(ExamAttemptCreateDto create)
        {
            if (!await examAttemptRepository.StudentExistsAsync(create.StudentId))
                throw new NotFoundException("Student not found.");

            if (!await examAttemptRepository.CourseExistsAsync(create.CourseId))
                throw new NotFoundException("Course not found.");

            var isEnrolled =
                await examAttemptRepository.IsStudentEnrolledInCourseAsync(
                    create.StudentId, create.CourseId);

            if (!isEnrolled)
                throw new ConflictException(
                    "The student must enroll in the course before attempting its exam.");

            if (!await examAttemptRepository.HasQuestionsAsync(create.CourseId))
                throw new ConflictException(
                    "This course does not have any exam questions yet.");

            var attempt = new ExamAttempt
            {
                StudentId = create.StudentId,
                CourseId = create.CourseId,
                Score = 0,
                IsSubmitted = false,
                StartedAt = DateTime.UtcNow
            };

               await examAttemptRepository.CreateExamAttemptAsync(attempt);

            logger.LogInformation(
                "Exam attempt {AttemptId} created for student {StudentId} in course {CourseId}.",
                attempt.Id, attempt.StudentId, attempt.CourseId);

            var saved =
                await examAttemptRepository.GetExamAttemptByIdAsync(attempt.Id);

            if (saved is null)
                throw new InvalidOperationException(
                    "The created exam attempt could not be retrieved.");

            return new ResponseModel<ExamAttemptDto>
            {
                Success = true,
                Message = "Exam attempt created successfully.",
                Data = MapToDto(saved)
            };
        }

        public async Task<ResponseModel<ExamAttemptDto>>
        GetExamAttemptByIdAsync(Guid id)
        {
            var attempt =
                await examAttemptRepository.GetExamAttemptByIdAsync(id);

            if (attempt is null)
                throw new NotFoundException("Exam attempt not found.");

            return new ResponseModel<ExamAttemptDto>
            {
                Success = true,
                Message = "Exam attempt retrieved successfully.",
                Data = MapToDto(attempt)
            };
        }

        public async Task<ResponseModel<PagedResponse<ExamAttemptDto>>>
        GetAllExamAttemptsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                throw new ArgumentException(
                    "Page number must be at least 1 and page size must be between 1 and 100.");

            var (attempts, totalCount) =
                await examAttemptRepository.GetAllExamAttemptsAsync(
                    pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            var pagedResponse = new PagedResponse<ExamAttemptDto>
            {
                Items = attempts.Select(MapToDto).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };

            return new ResponseModel<PagedResponse<ExamAttemptDto>>
            {
                Success = true,
                Message = "Exam attempts retrieved successfully.",
                Data = pagedResponse
            };
        }

        public async Task<ResponseModel<IEnumerable<ExamAttemptDto>>>
        GetExamAttemptsByStudentAsync(Guid studentId)
        {
            if (!await examAttemptRepository.StudentExistsAsync(studentId))
                throw new NotFoundException("Student not found.");

            var attempts =
                await examAttemptRepository.GetExamAttemptsByStudentAsync(studentId);

            return new ResponseModel<IEnumerable<ExamAttemptDto>>
            {
                Success = true,
                Message = "Student exam attempts retrieved successfully.",
                Data = attempts.Select(MapToDto).ToList()
            };
        }


        public async Task<ResponseModel<ExamAttemptDto>>
        SubmitExamAttemptAsync(Guid id)
        {
            var attempt =
                await examAttemptRepository.GetExamAttemptByIdAsync(id);

            if (attempt is null)
                throw new NotFoundException("Exam attempt not found.");

            if (attempt.IsSubmitted)
            {
                logger.LogWarning(
                    "Submit called for attempt {AttemptId} but it was already submitted at {SubmittedAt} by {StudentId} with score {Score}.",
                    attempt.Id, attempt.SubmittedAt, attempt.StudentId, attempt.Score);


                throw new ConflictException(
                      "This exam attempt has already been submitted");
            }

            if (!attempt.ExamAnswers.Any())
                throw new ConflictException(
                    "You must answer at least one question before submitting.");

            decimal score = 0;

            foreach (var answer in attempt.ExamAnswers)
            {
                var isCorrect = string.Equals(
                    answer.SelectedAnswer.Trim(),
                    answer.Question.CorrectAnswer.Trim(),
                    StringComparison.OrdinalIgnoreCase);

                answer.IsCorrect = isCorrect;

                if (isCorrect)
                    score += answer.Question.Marks;
            }

            attempt.Score = score;
            attempt.IsSubmitted = true;
            attempt.SubmittedAt = DateTime.UtcNow;

            var updated =
                await examAttemptRepository.UpdateExamAttemptAsync(attempt);

            if (!updated)
                throw new InvalidOperationException(
                    "The exam attempt could not be submitted.");

            logger.LogInformation(
                "Exam attempt {AttemptId} submitted. Score: {Score}.",
                attempt.Id, attempt.Score);

            return new ResponseModel<ExamAttemptDto>
            {
                Success = true,
                Message = "Exam submitted and scored successfully.",
                Data = MapToDto(attempt)
            };
        }

        private static ExamAttemptDto MapToDto(ExamAttempt attempt)
        {
            return new ExamAttemptDto
            {
                Id = attempt.Id,
                StudentId = attempt.StudentId,
                StudentName =
                    $"{attempt.Student.FirstName} {attempt.Student.LastName}",

                CourseId = attempt.CourseId,
                CourseCode = attempt.Course.CourseCode,
                CourseTitle = attempt.Course.CourseTitle,

                Score = attempt.Score,
                IsSubmitted = attempt.IsSubmitted,
                StartedAt = attempt.StartedAt,
                SubmittedAt = attempt.SubmittedAt
            };
        }
    }
}

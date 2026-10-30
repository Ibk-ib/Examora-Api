using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories
{
    public interface IExamAttemptRepository
    {
        Task<(IEnumerable<ExamAttempt> Attempts, int TotalCount)> GetAllExamAttemptsAsync(int pageNumber, int pageSize);
        Task<ExamAttempt?> GetExamAttemptByIdAsync(Guid id);
        Task<IEnumerable<ExamAttempt>> GetExamAttemptsByStudentAsync(Guid studentId);
        Task<bool> StudentExistsAsync(Guid studentId);
        Task<bool> CourseExistsAsync(Guid courseId);
        Task<bool> IsStudentEnrolledInCourseAsync(
        Guid studentId, Guid courseId);
        Task CreateExamAttemptAsync(ExamAttempt attempt);

        Task<bool> UpdateExamAttemptAsync(ExamAttempt attempt);

        Task<bool> HasQuestionsAsync(Guid courseId);

    }
}


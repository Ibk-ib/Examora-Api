using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services
{
    public interface IExamAttemptService
    {
        Task<ResponseModel<ExamAttemptDto>> CreateExamAttemptAsync(
        ExamAttemptCreateDto create);

        Task<ResponseModel<ExamAttemptDto>> GetExamAttemptByIdAsync(
            Guid id);

        Task<ResponseModel<PagedResponse<ExamAttemptDto>>>
            GetAllExamAttemptsAsync(int pageNumber, int pageSize);

        Task<ResponseModel<IEnumerable<ExamAttemptDto>>>
            GetExamAttemptsByStudentAsync(Guid studentId);

        Task<ResponseModel<ExamAttemptDto>> SubmitExamAttemptAsync(
            Guid id);
    }
}


using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public interface IExamAnswerRepository
{
    Task<ExamAnswer?> GetExamAnswerByIdAsync(Guid id);

    Task<IEnumerable<ExamAnswer>> GetAnswersByAttemptAsync(Guid attemptId);

    Task<bool> CreateExamAnswerAsync(ExamAnswer answer);

    Task<bool> UpdateExamAnswerAsync(ExamAnswer answer);

    Task<bool> DeleteExamAnswerAsync(ExamAnswer answer);

    Task<bool> AnswerExistsAsync(Guid attemptId, Guid questionId);

    Task<ExamAttempt?> GetExamAttemptByIdAsync(Guid attemptId);
}

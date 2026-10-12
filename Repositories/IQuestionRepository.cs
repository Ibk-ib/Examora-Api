
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public interface IQuestionRepository
{
    Task<(IEnumerable<Question> Questions, int TotalCount)>
        GetAllQuestionsAsync(int pageNumber, int pageSize);

    Task<Question?> GetQuestionByIdAsync(Guid id);

    Task<(IEnumerable<Question> Questions, int TotalCount)>
        GetQuestionsByCourseAsync(
            Guid courseId, int pageNumber, int pageSize);

    Task<bool> CreateQuestionAsync(Question question);

    Task<bool> UpdateQuestionAsync(Question question);

    Task<bool> DeleteQuestionAsync(Question question);

    Task<bool> HasExamAnswersAsync(Guid questionId);
}

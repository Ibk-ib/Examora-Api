
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services;

public interface IQuestionService
{
    Task<ResponseModel<bool>> CreateQuestionAsync(QuestionCreateDto create);

    Task<ResponseModel<bool>> UpdateQuestionAsync(
        Guid id, QuestionCreateDto update);

    Task<ResponseModel<bool>> DeleteQuestionAsync(Guid id);

    Task<ResponseModel<PagedResponse<QuestionDto>>> GetAllQuestionsAsync(
        int pageNumber, int pageSize);

    Task<ResponseModel<PagedResponse<QuestionDto>>> GetQuestionsByCourseAsync(
        Guid courseId, int pageNumber, int pageSize);

    Task<ResponseModel<QuestionDto?>> GetQuestionByIdAsync(Guid id);
}

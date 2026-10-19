
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services;

public interface IExamAnswerService
{
    Task<ResponseModel<ExamAnswerDto>> CreateExamAnswerAsync(
        ExamAnswerCreateDto create);

    Task<ResponseModel<ExamAnswerDto?>> GetExamAnswerByIdAsync(Guid id);

    Task<ResponseModel<IEnumerable<ExamAnswerDto>>> GetAnswersByAttemptAsync(
        Guid attemptId);

    Task<ResponseModel<bool>> UpdateExamAnswerAsync(
        Guid id, ExamAnswerCreateDto update);

    Task<ResponseModel<bool>> DeleteExamAnswerAsync(Guid id);
}

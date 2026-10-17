
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services;

public interface IResultService
{
    Task<ResponseModel<ResultDto>> GenerateResultAsync(
        ResultCreateDto create);

    Task<ResponseModel<ResultDto>> GetResultByIdAsync(Guid id);

    Task<ResponseModel<PagedResponse<ResultDto>>> GetAllResultsAsync(
        int pageNumber, int pageSize);

    Task<ResponseModel<IEnumerable<ResultDto>>> GetResultsByStudentAsync(
        Guid studentId);
}

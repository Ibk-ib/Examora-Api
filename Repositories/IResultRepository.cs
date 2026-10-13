
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public interface IResultRepository
{
    Task<(IEnumerable<Result> Results, int TotalCount)>
        GetAllResultsAsync(int pageNumber, int pageSize);

    Task<Result?> GetResultByIdAsync(Guid id);

    Task<IEnumerable<Result>> GetResultsByStudentAsync(Guid studentId);

    Task<Result?> GetResultByStudentAndCourseAsync(
        Guid studentId, Guid courseId);

    Task<decimal> GetTotalMarksForCourseAsync(Guid courseId);

    Task<bool> CreateResultAsync(Result result);
}

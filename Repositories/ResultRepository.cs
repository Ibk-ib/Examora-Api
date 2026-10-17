
using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public class ResultRepository(ApplicationDbContext context)
    : IResultRepository
{
    public async Task<(IEnumerable<Result> Results, int TotalCount)>
        GetAllResultsAsync(int pageNumber, int pageSize)
    {
        var query = context.Results
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Course)
            .OrderBy(x => x.Student.LastName)
            .ThenBy(x => x.Course.CourseCode);

        var totalCount = await query.CountAsync();

        var results = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (results, totalCount);
    }

    public Task<Result?> GetResultByIdAsync(Guid id)
    {
        return context.Results
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Result>> GetResultsByStudentAsync(
        Guid studentId)
    {
        return await context.Results
            .AsNoTracking()
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Where(x => x.StudentId == studentId)
            .OrderBy(x => x.Course.CourseCode)
            .ToListAsync();
    }

    public Task<Result?> GetResultByStudentAndCourseAsync(
        Guid studentId, Guid courseId)
    {
        return context.Results
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId &&
                x.CourseId == courseId);
    }

    public async Task<decimal> GetTotalMarksForCourseAsync(Guid courseId)
    {
        return await context.Questions
            .Where(x => x.CourseId == courseId)
            .SumAsync(x => (decimal?)x.Marks) ?? 0m;
    }

    public async Task<bool> CreateResultAsync(Result result)
    {
        await context.Results.AddAsync(result);
        return await context.SaveChangesAsync() > 0;
    }
}

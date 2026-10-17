
using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public class QuestionRepository(ApplicationDbContext context)
    : IQuestionRepository
{
    public async Task<(IEnumerable<Question> Questions, int TotalCount)>
        GetAllQuestionsAsync(int pageNumber, int pageSize)
    {
        var query = context.Questions
            .AsNoTracking()
            .Include(q => q.Course);

        var totalCount = await query.CountAsync();

        var questions = await query
            .OrderBy(q => q.Course.CourseCode)
            .ThenBy(q => q.QuestionText)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (questions, totalCount);
    }

    public async Task<Question?> GetQuestionByIdAsync(Guid id)
    {
        return await context.Questions
            .Include(q => q.Course)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<(IEnumerable<Question> Questions, int TotalCount)>
        GetQuestionsByCourseAsync(
            Guid courseId, int pageNumber, int pageSize)
    {
        var query = context.Questions
            .AsNoTracking()
            .Where(q => q.CourseId == courseId)
            .Include(q => q.Course);

        var totalCount = await query.CountAsync();

        var questions = await query
            .OrderBy(q => q.QuestionText)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (questions, totalCount);
    }

    public async Task<bool> CreateQuestionAsync(Question question)
    {
        await context.Questions.AddAsync(question);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateQuestionAsync(Question question)
    {
        context.Questions.Update(question);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteQuestionAsync(Question question)
    {
        context.Questions.Remove(question);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> HasExamAnswersAsync(Guid questionId)
    {
        return await context.ExamAnswers
            .AnyAsync(a => a.QuestionId == questionId);
    }
}

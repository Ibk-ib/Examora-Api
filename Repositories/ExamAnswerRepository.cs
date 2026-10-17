
using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public class ExamAnswerRepository(ApplicationDbContext context)
    : IExamAnswerRepository
{
    public async Task<ExamAnswer?> GetExamAnswerByIdAsync(Guid id)
    {
        return await context.ExamAnswers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<ExamAnswer>> GetAnswersByAttemptAsync(
        Guid attemptId)
    {
        return await context.ExamAnswers
            .AsNoTracking()
            .Include(a => a.Question)
            .Where(a => a.ExamAttemptId == attemptId)
            .OrderBy(a => a.Question.QuestionText)
            .ToListAsync();
    }

    public async Task<bool> CreateExamAnswerAsync(ExamAnswer answer)
    {
        await context.ExamAnswers.AddAsync(answer);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateExamAnswerAsync(ExamAnswer answer)
    {
        context.ExamAnswers.Update(answer);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteExamAnswerAsync(ExamAnswer answer)
    {
        context.ExamAnswers.Remove(answer);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AnswerExistsAsync(
        Guid attemptId, Guid questionId)
    {
        return await context.ExamAnswers.AnyAsync(a =>
            a.ExamAttemptId == attemptId &&
            a.QuestionId == questionId);
    }

    public async Task<ExamAttempt?> GetExamAttemptByIdAsync(
        Guid attemptId)
    {
        return await context.ExamAttempts
            .FirstOrDefaultAsync(a => a.Id == attemptId);
    }
}

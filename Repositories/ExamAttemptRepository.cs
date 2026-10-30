using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Data;
using Microsoft.EntityFrameworkCore;

namespace UniversityAcademicApi.Repositories
{
    public class ExamAttemptRepository(ApplicationDbContext context) : IExamAttemptRepository
    {
        public async Task<(IEnumerable<ExamAttempt> Attempts, int TotalCount)> GetAllExamAttemptsAsync(int pageNumber, int pageSize) {

            var result = context.ExamAttempts
                .AsNoTracking()
                .Include(ExamAtt => ExamAtt.Student)
                .Include(ExamAtt => ExamAtt.Course)
                .OrderByDescending(ExamAtt => ExamAtt.StartedAt);

            var totalCount = await result.CountAsync();

            var attempt = await result
                .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

            return (attempt, totalCount);
        }

        public async Task<ExamAttempt?> GetExamAttemptByIdAsync(Guid id) {

            return await context.ExamAttempts
                .Include(ExamAtt => ExamAtt.Student)
                .Include(ExamAtt => ExamAtt.Course)
                .Include(ExamAtt => ExamAtt.ExamAnswers)
                .ThenInclude(ExamAtt => ExamAtt.Question)
                .FirstOrDefaultAsync(ExamAtt => ExamAtt.Id == id);
        }

        public async Task<IEnumerable<ExamAttempt>> GetExamAttemptsByStudentAsync(Guid studentId) {

            return await context.ExamAttempts
            .AsNoTracking()
            .Include(ExamAtt => ExamAtt.Student)
            .Include(ExamAtt => ExamAtt.Course)
            .Where(ExamAtt => ExamAtt.StudentId == studentId)
            .OrderByDescending(x => x.StartedAt)
            .ToListAsync();
        }

        public Task<bool> StudentExistsAsync(Guid studentId)
        {
            return context.Students.AnyAsync(Student => Student.Id == studentId);
        }

        public Task<bool> CourseExistsAsync(Guid courseId)
        {
            return context.Courses.AnyAsync(Course => Course.Id == courseId);
        }

        public Task<bool> IsStudentEnrolledInCourseAsync(
        Guid studentId, Guid courseId)
        {
            return context.Enrollments.AnyAsync(x =>
                x.StudentId == studentId &&
                x.CourseId == courseId);
        }

        public async Task CreateExamAttemptAsync(ExamAttempt attempt)
        {
            await context.ExamAttempts.AddAsync(attempt);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateExamAttemptAsync(ExamAttempt attempt)
        {
            context.ExamAttempts.Update(attempt);
            return await context.SaveChangesAsync() > 0;
        }

        public Task<bool> HasQuestionsAsync(Guid courseId)
        {
            return context.Questions.AnyAsync(x => x.CourseId == courseId);
        }

    }
}

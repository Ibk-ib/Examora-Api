using UniversityAcademicApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
namespace UniversityAcademicApi.Repositories;

    public class CourseRepository(ApplicationDbContext context) : ICourseRepository
    {
        public async Task<IEnumerable<Course>> GetAllCoursesAsync() {

            return await context.Courses
                 .AsNoTracking()
                 .OrderBy(c => c.CourseCode)
                 .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(Guid Id) {

            return await context.Courses
                .FirstOrDefaultAsync(c => c.Id == Id);

        }

        public async Task<Course> CreateCourseAsync(Course course) {

            await context.AddAsync(course);
            await context.SaveChangesAsync();

            return course;
        }

        public async Task<Course?> UpdateCourseAsync(Course course)
        {
            context.Courses.Update(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> DeleteCourseAsync(Course course) {
            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return course;
        }
        
            public async Task<bool> CourseCodeExistsAsync(string courseCode, Guid? excludeCourseId = null)
        {
            return await context.Courses
                .AnyAsync(c => c.CourseCode == courseCode && 
                (!excludeCourseId.HasValue || c.Id != excludeCourseId.Value));
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid courseId)
        {
            var hasEnrollments = await context.Enrollments
                .AnyAsync(enrollment => enrollment.CourseId == courseId);

            var hasQuestions = await context.Questions
                .AnyAsync(question => question.CourseId == courseId);

            var hasExamAttempts = await context.ExamAttempts
                .AnyAsync(attempt => attempt.CourseId == courseId);

            var hasResults = await context.Results
                .AnyAsync(result => result.CourseId == courseId);

            return hasEnrollments ||
                   hasQuestions ||
                   hasExamAttempts ||
                   hasResults;
        }


    }


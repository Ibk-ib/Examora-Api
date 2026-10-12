using UniversityAcademicApi.Data;
using Microsoft.EntityFrameworkCore;
namespace UniversityAcademicApi.Repositories;

    public class CourseRepository(ApplicationDbContext context) : ICourseRepository
    {
        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetAllCoursesAsync(
            int
            pageNumber,
            int pageSize){

            var courses = await context.Courses
                 .AsNoTracking()
                 .OrderBy(c => c.CourseCode)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

            var totalCount = await context.Courses.CountAsync();

            return (courses, totalCount);
        }

        public async Task<Course?> GetCourseByIdAsync(Guid Id) {

            return await context.Courses
                .FirstOrDefaultAsync(c => c.Id == Id);

        }

        public async Task<bool> CreateCourseAsync(Course course) {

            await context.AddAsync(course);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            context.Courses.Update(course);
            return await context.SaveChangesAsync() > 0;
            
        }

        public async Task<bool> DeleteCourseAsync(Course course) {
            context.Courses.Remove(course);
            return await context.SaveChangesAsync() > 0;
            
        }
        
            public async Task<bool> CourseCodeExistsAsync(string courseCode, string courseTitle, Guid? excludeCourseId = null)
        {
            return await context.Courses
                .AnyAsync(c => (c.CourseCode == courseCode || c.CourseTitle == courseTitle) && 
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


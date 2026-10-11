using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories
{
    public interface ICourseRepository
    {
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetAllCoursesAsync(
            int
            pageNumber,
            int pageSize);
        Task<Course?> GetCourseByIdAsync(Guid Id);
        Task<bool> CreateCourseAsync(Course course);
        Task<bool> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(Course course);
        Task<bool> CourseCodeExistsAsync(
        string courseCode,
        Guid? excludeCourseId = null);
        Task<bool> HasRelatedRecordsAsync(Guid Id);
    }
}
  
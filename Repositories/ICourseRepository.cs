using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories
{
    public interface ICourseRepository
    {
        Task <IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(Guid Id);
        Task<Course> CreateCourseAsync(Course course);
        Task<Course?> UpdateCourseAsync(Course course);
        Task<Course?> DeleteCourseAsync(Course course);
        Task<bool> CourseCodeExistsAsync(
        string courseCode,
        Guid? excludeCourseId = null);
        Task<bool> HasRelatedRecordsAsync(Guid Id);
    }
}
  
using UniversityAcademicApi.Data;
namespace UniversityAcademicApi.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<(IEnumerable<Enrollment>, int TotalCount)> GetAllEnrollmentsAsync(int pageNumber, int pageSize);

        Task<Enrollment?> GetEnrollmentByIdAsync(Guid id);

        Task<bool> CreateEnrollmentAsync(Enrollment enrollment);

        Task<bool> UpdateEnrollmentAsync(Enrollment enrollment);

        Task<bool> DeleteEnrollmentAsync(Enrollment enrollment);

        Task<bool> EnrollmentExistAsync(Guid studentId, Guid courseId, Guid? excludeEnrollmentId = null);
    }
}

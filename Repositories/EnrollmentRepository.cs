using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories
{
    public class EnrollmentRepository(ApplicationDbContext context) : IEnrollmentRepository
    {
        public async Task<(IEnumerable<Enrollment> Enrollments, int TotalCount)> GetAllEnrollmentsAsync(int pageNumber, int pageSize) {
            var include = context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course);

            var totalCount = await include.CountAsync();

            var enrollments = await include
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (enrollments, totalCount);
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(Guid id) {

            return await context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> CreateEnrollmentAsync(Enrollment enrollment) { 

            await context.Enrollments.AddAsync(enrollment);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEnrollmentAsync(Enrollment enrollment) {

            context.Enrollments.Update(enrollment);
            return await context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteEnrollmentAsync(Enrollment enrollment) {

            context.Enrollments.Remove(enrollment);
            return await context.SaveChangesAsync() > 0;
        
        }

        public async Task<bool> EnrollmentExistAsync(Guid studentId, Guid courseId, Guid? excludeEnrollmentId = null) {

            return await context.Enrollments.AnyAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId &&
                (!excludeEnrollmentId.HasValue || e.Id != excludeEnrollmentId.Value)
            );
        
        }


        public async Task<(IEnumerable<Enrollment> Enrollments, int TotalCount)>
    GetEnrollmentsByStudentIdAsync(Guid studentId, int pageNumber, int pageSize)
        {
            var query = context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .OrderBy(e => e.Id);

            var totalCount = await query.CountAsync();

            var enrollments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (enrollments, totalCount);
        }

    }

}

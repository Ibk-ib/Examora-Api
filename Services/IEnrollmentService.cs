using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services
{
    public interface IEnrollmentService
    {
        Task<ResponseModel<bool>> CreateEnrollmentAsync(EnrollmentCreateDto create);
        Task<ResponseModel<bool>> UpdateEnrollmentAsync(Guid id, EnrollmentCreateDto update);
        Task<ResponseModel<bool>> DeleteEnrollmentAsync(Guid id);
        Task<ResponseModel<PagedResponse<EnrollmentDto>>> GetAllEnrollmentsAsync(int pageNumber, int pageSize);
        Task<ResponseModel<EnrollmentDto?>> GetEnrollmentByIdAsync(Guid id);
        Task<ResponseModel<PagedResponse<EnrollmentDto>>>
        GetEnrollmentsByStudentIdAsync(Guid studentId, int pageNumber, int pageSize);
    }
}

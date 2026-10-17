
using UniversityAcademicApi.DTOs;

namespace UniversityAcademicApi.Services
{
    public interface IAuthService
    {
        Task<ResponseModel<AuthResponseDto>> RegisterStudentAsync(
            StudentRegisterDto dto);

        Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}

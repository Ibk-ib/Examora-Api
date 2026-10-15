
using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Student student);
    }
}

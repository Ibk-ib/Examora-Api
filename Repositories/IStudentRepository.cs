using UniversityAcademicApi.Data;

namespace UniversityAcademicApi.Repositories;

public interface IStudentRepository
{

    Task<bool> CreateStudentAsync(Student student);
    Task <bool> DeleteStudentAsync(Student student);
    Task<(IEnumerable<Student> Students, int TotalCount)> GetAllStudentsAsync(
        int 
        pageNumber,
        int pageSize);
    Task<Student?> GetStudentByIdAsync(Guid id);

    Task<bool> UpdateStudentAsync(Student student);
    
    Task<Student?> GetStudentByMatricNumberAsync(string matricNumber);
}

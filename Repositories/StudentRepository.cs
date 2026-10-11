using UniversityAcademicApi.Data;
using Microsoft.EntityFrameworkCore;

namespace UniversityAcademicApi.Repositories;

public class StudentRepository(ApplicationDbContext dbContext) : IStudentRepository
{
    public async Task<bool> CreateStudentAsync(Student student)
    {
        await dbContext.AddAsync(student);

        return await dbContext.SaveChangesAsync() > 0
            ? true : false;
    }

    public async Task<bool> DeleteStudentAsync(Student student)
    {
        dbContext.Students.Remove(student);
        return await dbContext.SaveChangesAsync() > 0
            ? true : false;
    }

    public async Task<(IEnumerable<Student> Students, int TotalCount)> GetAllStudentsAsync(
    int pageNumber,
    int pageSize)
    {
        var totalCount = await dbContext.Students.CountAsync();

        var students = await dbContext.Students
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (students, totalCount);
    }


    public async Task<Student?> GetStudentByIdAsync(Guid id)
    {
        return await dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student?> GetStudentByMatricNumberAsync(string matricNumber)
    {
        return await dbContext.Students.FirstOrDefaultAsync(s => s.MatricNumber == matricNumber);
    }
    

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        dbContext.Students.Update(student);
        return await dbContext.SaveChangesAsync() > 0
            ? true : false;
    }
}

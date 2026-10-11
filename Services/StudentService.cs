using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Repositories;
using UniversityAcademicApi.Exceptions;
namespace UniversityAcademicApi.Services;

public class StudentService(
    IStudentRepository studentRepository,
    ILogger<StudentService> logger)
    : IStudentService
{
    public async Task<ResponseModel<bool>> CreateStudentAsync(StudentCreateDto request)
    {
        logger.LogInformation(
               "Creating student with matric number {MatricNumber}.",
               request.MatricNumber);

        var checkIfStudentExists = await studentRepository.GetStudentByMatricNumberAsync(request.MatricNumber);

            if (checkIfStudentExists != null)
            {

            logger.LogWarning(
              "Student with matric number {MatricNumber} already exists.",
              request.MatricNumber);

            throw new ConflictException("Student with the provided matric number already exists.");
            }

            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                MatricNumber = request.MatricNumber,
                Department = request.Department,
                Level = request.Level
            };

            var created = await studentRepository.CreateStudentAsync(student);
            

            return new ResponseModel<bool>
            {
                Success = created,
                Message = created ? "Student created successfully." : "Failed to create student.",
                Data = created
            };
    }

    public async Task<ResponseModel<bool>> DeleteStudentAsync(Guid id)
    {
        var student = await studentRepository.GetStudentByIdAsync(id);
        if (student == null)
        {
            throw new NotFoundException("Student not found.");
        }

        var result = await studentRepository.DeleteStudentAsync(student);

        return new ResponseModel<bool>
        {
            Success = result,
            Message = result ? "Student deleted successfully." : "Failed to delete student.",
            Data = result
        };
    }

    public async Task<ResponseModel<PagedResponse<StudentDto>>> GetAllStudentsAsync(
    int pageNumber,
    int pageSize)
    {
        var result = await studentRepository.GetAllStudentsAsync(pageNumber, pageSize);

        var studentDtos = result.Students.Select(s => new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            MatricNumber = s.MatricNumber,
            Department = s.Department,
            Level = s.Level
        }).ToList();

        var totalPages = (int)Math.Ceiling(
            result.TotalCount / (double)pageSize);

        var pagedResponse = new PagedResponse<StudentDto>
        {
            Items = studentDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = result.TotalCount,
            TotalPages = totalPages,
            HasNextPage = pageNumber < totalPages,
            HasPreviousPage = pageNumber > 1
        };

        return new ResponseModel<PagedResponse<StudentDto>>
        {
            Success = true,
            Message = "Students retrieved successfully.",
            Data = pagedResponse
        };
    }

    public async Task<ResponseModel<StudentDto?>> GetStudentByIdAsync(Guid id)
    {
        var student = await studentRepository.GetStudentByIdAsync(id);
        if (student == null)
        {
            throw new NotFoundException("Student not found.");
        }

        var studentDto = new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            MatricNumber = student.MatricNumber,
            Department = student.Department,
            Level = student.Level
        };

        return new ResponseModel<StudentDto?>
        {
            Success = true,
            Message = "Student retrieved successfully.",
            Data = studentDto
        };
    }


    public async Task<ResponseModel<StudentDto?>> GetStudentByMatricNumberAsync(string matricNumber)
    {
        var student = await studentRepository.GetStudentByMatricNumberAsync(matricNumber);
        if (student == null)
        {
            throw new NotFoundException("Student not found.");
        }

        var studentDto = new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            MatricNumber = student.MatricNumber,
            Department = student.Department,
            Level = student.Level
        };

        return new ResponseModel<StudentDto?>
        {
            Success = true,
            Message = "Student retrieved successfully.",
            Data = studentDto
        };
    }

    public async Task<ResponseModel<bool>> UpdateStudentAsync(Guid id, StudentCreateDto request)
    {
        var student = await studentRepository.GetStudentByIdAsync(id);
        if (student == null)
        {
            throw new NotFoundException("Student not found.");
        }

        // Update the student properties
        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.Email = request.Email;
        student.Department = request.Department;
        student.Level = request.Level;

        var result = await studentRepository.UpdateStudentAsync(student);

        return new ResponseModel<bool>
        {
            Success = result,
            Message = result ? "Student updated successfully." : "Failed to update student.",
            Data = result
        };
    }
}


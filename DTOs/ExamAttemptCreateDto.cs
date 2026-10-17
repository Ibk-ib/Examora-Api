
namespace UniversityAcademicApi.DTOs;

public class ExamAttemptCreateDto
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}

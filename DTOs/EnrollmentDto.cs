

namespace UniversityAcademicApi.DTOs;

public class EnrollmentDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public Guid CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }
}

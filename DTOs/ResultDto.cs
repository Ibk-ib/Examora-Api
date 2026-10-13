
namespace UniversityAcademicApi.DTOs;

public class ResultDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;

    public Guid CourseId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;

    public decimal Score { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal Percentage { get; set; }

    public string Grade { get; set; } = string.Empty;
    public decimal GradePoint { get; set; }
    public int CreditUnit { get; set; }
}

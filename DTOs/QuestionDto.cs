
namespace UniversityAcademicApi.DTOs;

public class QuestionDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string QuestionText { get; set; } = string.Empty;

    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;

    public decimal Marks { get; set; }
}

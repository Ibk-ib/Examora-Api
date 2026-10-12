
namespace UniversityAcademicApi.DTOs;

public class QuestionCreateDto
{
    public Guid CourseId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;

    public string CorrectAnswer { get; set; } = string.Empty;

    public decimal Marks { get; set; } = 1m;
}

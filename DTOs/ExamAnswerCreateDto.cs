
namespace UniversityAcademicApi.DTOs;

public class ExamAnswerCreateDto
{
    public Guid ExamAttemptId { get; set; }

    public Guid QuestionId { get; set; }

    public string SelectedAnswer { get; set; } = string.Empty;
}


namespace UniversityAcademicApi.DTOs;

public class ExamAnswerDto
{
    public Guid Id { get; set; }

    public Guid ExamAttemptId { get; set; }

    public Guid QuestionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string SelectedAnswer { get; set; } = string.Empty;

    //public bool IsCorrect { get; set; }
}

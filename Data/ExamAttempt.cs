namespace UniversityAcademicApi.Data
{
    public class ExamAttempt : BaseEntity
    {
            public Guid StudentId { get; set; }

            public Guid CourseId { get; set; }

            public DateTime StartedAt { get; set; }

            public DateTime? SubmittedAt { get; set; }
            public bool IsSubmitted { get; set; } = false;

            public decimal Score { get; set; }
            public Student Student { get; set; } = null!;
            public Course Course { get; set; } = null!;
            public ICollection<ExamAnswer> ExamAnswers { get; set; } = new List<ExamAnswer>();
    }
}

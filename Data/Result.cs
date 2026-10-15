namespace UniversityAcademicApi.Data
{
    public class Result : BaseEntity
    {
        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }

        public decimal Score { get; set; }

        public string Grade { get; set; } = string.Empty;

        public decimal GradePoint { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public int CreditUnit { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}

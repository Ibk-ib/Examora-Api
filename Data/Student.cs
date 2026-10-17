
namespace UniversityAcademicApi.Data
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MatricNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public int Level { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Student";

        public ICollection<Enrollment> Enrollments { get; set; }
            = new List<Enrollment>();

        public ICollection<ExamAttempt> ExamAttempts { get; set; }
            = new List<ExamAttempt>();

        public ICollection<Result> Results { get; set; }
            = new List<Result>();
    }
}

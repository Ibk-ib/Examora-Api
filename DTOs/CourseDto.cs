namespace UniversityAcademicApi.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CreditUnit { get; set; }

        public string Department { get; set; } = string.Empty;

        public int Level { get; set; }
    }
}

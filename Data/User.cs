using UniversityAcademicApi.Data;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Student";

    public Guid? StudentId { get; set; }

    public Student? Student { get; set; }
}
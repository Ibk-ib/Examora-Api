namespace UniversityAcademicApi.DTOs;

public record StudentDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MatricNumber { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int Level { get; set; }
}
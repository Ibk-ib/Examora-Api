using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Data.Seeding;

public static class AdminSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        IPasswordHashingService passwordHashingService,
        IConfiguration configuration)
    {
        var matricNumber =
            configuration["AdminSeed:MatricNumber"];

        var password =
            configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(matricNumber) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Admin seed credentials are not configured.");
        }

        matricNumber = matricNumber.Trim().ToUpperInvariant();

        var existingStudent = await context.Students
            .SingleOrDefaultAsync(s =>
                s.MatricNumber == matricNumber);

        if (existingStudent is not null)
        {
            if (existingStudent.Role != "Admin")
            {
                throw new InvalidOperationException(
                    "The configured admin matric number already belongs to a non-admin account.");
            }

            return;
        }

        var admin = new Student
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = "admin@localhost.invalid",
            MatricNumber = matricNumber,
            Department = "Administration",
            Role = "Admin",
            PasswordHash =
                passwordHashingService.HashPassword(password)
        };

        context.Students.Add(admin);
        await context.SaveChangesAsync();
    }
}
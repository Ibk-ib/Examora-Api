using Microsoft.EntityFrameworkCore;
using UniversityAcademicApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ExamAttempt> ExamAttempts { get; set; }
    public DbSet<ExamAnswer> ExamAnswers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Result> Results { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Course)
            .WithMany(c => c.Questions)
            .HasForeignKey(q => q.CourseId);

        modelBuilder.Entity<ExamAttempt>()
            .HasOne(e => e.Student)
            .WithMany(s => s.ExamAttempts)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<ExamAttempt>()
            .HasOne(e => e.Course)
            .WithMany(c => c.ExamAttempts)
            .HasForeignKey(e => e.CourseId);

        modelBuilder.Entity<ExamAnswer>()
            .HasOne(a => a.ExamAttempt)
            .WithMany(e => e.ExamAnswers)
            .HasForeignKey(a => a.ExamAttemptId);

        modelBuilder.Entity<ExamAnswer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.ExamAnswers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Result>()
            .HasOne(r => r.Student)
            .WithMany(s => s.Results)
            .HasForeignKey(r => r.StudentId);

        modelBuilder.Entity<Result>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Results)
            .HasForeignKey(r => r.CourseId);

       


        modelBuilder.Entity<ExamAttempt>()
            .Property(e => e.Score)
            .HasPrecision(5, 2);

        modelBuilder.Entity<ExamAttempt>()
            .Property(e => e.GradePoint)
            .HasPrecision(3, 2);

        modelBuilder.Entity<Result>()
            .Property(r => r.Score)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Result>()
            .Property(r => r.GradePoint)
            .HasPrecision(3, 2);

        modelBuilder.Entity<Enrollment>()
        .HasIndex(e => new { e.StudentId, e.CourseId })
        .IsUnique();

    }
}

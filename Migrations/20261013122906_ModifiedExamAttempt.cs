using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityAcademicApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedExamAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "GradePoint",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "ExamAttempts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "ExamAttempts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "GradePoint",
                table: "ExamAttempts",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}

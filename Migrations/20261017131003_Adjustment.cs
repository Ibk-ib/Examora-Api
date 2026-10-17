using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityAcademicApi.Migrations
{
    /// <inheritdoc />
    public partial class Adjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

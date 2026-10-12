using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityAcademicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionAndExamAnswerModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_Questions_QuestionId",
                table: "ExamAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_ExamAnswers_ExamAttemptId",
                table: "ExamAnswers");

            migrationBuilder.AddColumn<decimal>(
                name: "Marks",
                table: "Questions",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswers_ExamAttemptId_QuestionId",
                table: "ExamAnswers",
                columns: new[] { "ExamAttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_Questions_QuestionId",
                table: "ExamAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAnswers_Questions_QuestionId",
                table: "ExamAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_ExamAnswers_ExamAttemptId_QuestionId",
                table: "ExamAnswers");

            migrationBuilder.DropColumn(
                name: "Marks",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswers_ExamAttemptId",
                table: "ExamAnswers",
                column: "ExamAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_ExamAttempts_ExamAttemptId",
                table: "ExamAnswers",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAnswers_Questions_QuestionId",
                table: "ExamAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

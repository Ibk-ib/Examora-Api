
using Microsoft.Extensions.Logging;
using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;

namespace UniversityAcademicApi.Services;

public class ResultService(
    IResultRepository resultRepository,
    IExamAttemptRepository examAttemptRepository,
    ILogger<ResultService> logger)
    : IResultService
{
    public async Task<ResponseModel<ResultDto>> GenerateResultAsync(
        ResultCreateDto create)
    {
        var attempt =
            await examAttemptRepository.GetExamAttemptByIdAsync(
                create.ExamAttemptId);
        if (attempt is null)
            throw new NotFoundException("Exam attempt not found.");

        if (!attempt.IsSubmitted)
            throw new ConflictException(
                "The exam must be submitted before a result can be generated.");

        var existing =
            await resultRepository.GetResultByStudentAndCourseAsync(
                attempt.StudentId, attempt.CourseId);

        if (existing is not null)
            throw new ConflictException(
                "A result already exists for this student and course.");

        var totalMarks =
            await resultRepository.GetTotalMarksForCourseAsync(
                attempt.CourseId);

        if (totalMarks <= 0)
            throw new ConflictException(
                "The course has no valid marks available.");

        var score = attempt.Score;
        var percentage = Math.Round(
            score / totalMarks * 100m, 2);

        var (grade, gradePoint) = CalculateGrade(percentage);

        var result = new Result
        {
            StudentId = attempt.StudentId,
            CourseId = attempt.CourseId,
            Score = score,
            Grade = grade,
            GradePoint = gradePoint,
            CreditUnit = attempt.Course.CreditUnit,
            StudentName = attempt.Student.FirstName
        };

        var created = await resultRepository.CreateResultAsync(result);

        if (!created)
            throw new InvalidOperationException(
                "The result could not be saved.");

        logger.LogInformation(
            "Result {ResultId} generated for student {StudentId}, course {CourseId}, percentage {Percentage}.",
            result.Id, result.StudentId, result.CourseId, percentage);

        var saved = await resultRepository.GetResultByIdAsync(result.Id);

        if (saved is null)
            throw new InvalidOperationException(
                "The saved result could not be retrieved.");

        return new ResponseModel<ResultDto>
        {
            Success = true,
            Message = "Result generated successfully.",
            Data = await MapToDtoAsync(saved)
        };
    }

    public async Task<ResponseModel<ResultDto>> GetResultByIdAsync(
        Guid id)
    {
        var result = await resultRepository.GetResultByIdAsync(id);

        if (result is null)
            throw new NotFoundException("Result not found.");

        return new ResponseModel<ResultDto>
        {
            Success = true,
            Message = "Result retrieved successfully.",
            Data = await MapToDtoAsync(result)
        };
    }

    public async Task<ResponseModel<PagedResponse<ResultDto>>>
        GetAllResultsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            throw new ArgumentException(
                "Page number must be at least 1 and page size must be between 1 and 100.");

        var (results, totalCount) =
            await resultRepository.GetAllResultsAsync(
                pageNumber, pageSize);

        var items = new List<ResultDto>();

        foreach (var result in results)
            items.Add(await MapToDtoAsync(result));

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new ResponseModel<PagedResponse<ResultDto>>
        {
            Success = true,
            Message = "Results retrieved successfully.",
            Data = new PagedResponse<ResultDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            }
        };
    }

    public async Task<ResponseModel<IEnumerable<ResultDto>>>
        GetResultsByStudentAsync(Guid studentId)
    {
        if (!await examAttemptRepository.StudentExistsAsync(studentId))
            throw new NotFoundException("Student not found.");

        var results =
            await resultRepository.GetResultsByStudentAsync(studentId);

        var items = new List<ResultDto>();

        foreach (var result in results)
            items.Add(await MapToDtoAsync(result));

        return new ResponseModel<IEnumerable<ResultDto>>
        {
            Success = true,
            Message = "Student results retrieved successfully.",
            Data = items
        };
    }

    private static (string Grade, decimal GradePoint)
        CalculateGrade(decimal percentage)
    {
        
        return percentage switch
        {
            >= 70m => ("A", 5m),
            >= 60m => ("B", 4m),
            >= 50m => ("C", 3m),
            >= 45m => ("D", 2m),
            >= 40m => ("E", 1m),
            _ => ("F", 0m)
        };
    }

    private async Task<ResultDto> MapToDtoAsync(Result result)
    {
        var totalMarks =
            await resultRepository.GetTotalMarksForCourseAsync(
                result.CourseId);

        var percentage = totalMarks > 0
            ? Math.Round(result.Score / totalMarks * 100m, 2)
            : 0m;

        return new ResultDto
        {
            Id = result.Id,
            StudentId = result.StudentId,
            StudentName =
                $"{result.Student.FirstName} {result.Student.LastName}",
            CourseId = result.CourseId,
            CourseCode = result.Course.CourseCode,
            CourseTitle = result.Course.CourseTitle,
            Score = result.Score,
            TotalMarks = totalMarks,
            Percentage = percentage,
            Grade = result.Grade,
            GradePoint = result.GradePoint,
            CreditUnit = result.CreditUnit
        };
    }
}

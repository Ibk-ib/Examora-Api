
using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;

namespace UniversityAcademicApi.Services;

public class QuestionService(
    IQuestionRepository questionRepository,
    ICourseRepository courseRepository,
    ILogger<QuestionService> logger)
    : IQuestionService
{
    public async Task<ResponseModel<bool>> CreateQuestionAsync(
        QuestionCreateDto create)
    {
        logger.LogInformation(
            "Creating question for course {CourseId}", create.CourseId);

        var course = await courseRepository.GetCourseByIdAsync(create.CourseId);

        if (course is null)
            throw new NotFoundException("Course not found.");

        var question = new Question
        {
            CourseId = create.CourseId,
            QuestionText = create.QuestionText.Trim(),
            OptionA = create.OptionA.Trim(),
            OptionB = create.OptionB.Trim(),
            OptionC = create.OptionC.Trim(),
            OptionD = create.OptionD.Trim(),
            CorrectAnswer = create.CorrectAnswer.Trim().ToUpperInvariant(),
            Marks = create.Marks
        };

        var created = await questionRepository.CreateQuestionAsync(question);

        if (!created)
            throw new InvalidOperationException("Failed to create question.");

        logger.LogInformation(
            "Question {QuestionId} created successfully", question.Id);

        return new ResponseModel<bool>
        {
            Success = true,
            Message = "Question created successfully.",
            Data = true
        };
    }

    public async Task<ResponseModel<bool>> UpdateQuestionAsync(
        Guid id, QuestionCreateDto update)
    {
        var question = await questionRepository.GetQuestionByIdAsync(id);

        if (question is null)
            throw new NotFoundException("Question not found.");

        var course = await courseRepository.GetCourseByIdAsync(update.CourseId);

        if (course is null)
            throw new NotFoundException("Course not found.");

        // Avoid changing a question that already has submitted answers.
        if (await questionRepository.HasExamAnswersAsync(id))
        {
            throw new ConflictException(
                "This question already has submitted answers and cannot be edited.");
        }

        question.CourseId = update.CourseId;
        question.QuestionText = update.QuestionText.Trim();
        question.OptionA = update.OptionA.Trim();
        question.OptionB = update.OptionB.Trim();
        question.OptionC = update.OptionC.Trim();
        question.OptionD = update.OptionD.Trim();
        question.CorrectAnswer =
            update.CorrectAnswer.Trim().ToUpperInvariant();
        question.Marks = update.Marks;

        var updated = await questionRepository.UpdateQuestionAsync(question);

        if (!updated)
            throw new InvalidOperationException("Failed to update question.");

        return new ResponseModel<bool>
        {
            Success = true,
            Message = "Question updated successfully.",
            Data = true
        };
    }

    public async Task<ResponseModel<bool>> DeleteQuestionAsync(Guid id)
    {
        var question = await questionRepository.GetQuestionByIdAsync(id);

        if (question is null)
            throw new NotFoundException("Question not found.");

        if (await questionRepository.HasExamAnswersAsync(id))
        {
            throw new ConflictException(
                "This question has submitted answers and cannot be deleted.");
        }

        var deleted = await questionRepository.DeleteQuestionAsync(question);

        if (!deleted)
            throw new InvalidOperationException("Failed to delete question.");

        return new ResponseModel<bool>
        {
            Success = true,
            Message = "Question deleted successfully.",
            Data = true
        };
    }

    public async Task<ResponseModel<PagedResponse<QuestionDto>>>
        GetAllQuestionsAsync(int pageNumber, int pageSize)
    {
        var result = await questionRepository.GetAllQuestionsAsync(
            pageNumber, pageSize);

        var items = result.Questions.Select(MapToDto).ToList();

        return BuildPagedResponse(items, result.TotalCount, pageNumber, pageSize);
    }

    public async Task<ResponseModel<PagedResponse<QuestionDto>>>
        GetQuestionsByCourseAsync(
            Guid courseId, int pageNumber, int pageSize)
    {
        var course = await courseRepository.GetCourseByIdAsync(courseId);

        if (course is null)
            throw new NotFoundException("Course not found.");

        var result = await questionRepository.GetQuestionsByCourseAsync(
            courseId, pageNumber, pageSize);

        var items = result.Questions.Select(MapToDto).ToList();

        return BuildPagedResponse(items, result.TotalCount, pageNumber, pageSize);
    }

    public async Task<ResponseModel<QuestionDto?>> GetQuestionByIdAsync(
        Guid id)
    {
        var question = await questionRepository.GetQuestionByIdAsync(id);

        if (question is null)
            throw new NotFoundException("Question not found.");

        return new ResponseModel<QuestionDto?>
        {
            Success = true,
            Message = "Question retrieved successfully.",
            Data = MapToDto(question)
        };
    }

    private static QuestionDto MapToDto(Question question)
    {
        return new QuestionDto
        {
            Id = question.Id,
            CourseId = question.CourseId,
            CourseCode = question.Course.CourseCode,
            QuestionText = question.QuestionText,
            OptionA = question.OptionA,
            OptionB = question.OptionB,
            OptionC = question.OptionC,
            OptionD = question.OptionD,
            Marks = question.Marks
        };
    }

    private static ResponseModel<PagedResponse<QuestionDto>>
        BuildPagedResponse(
            List<QuestionDto> items,
            int totalCount,
            int pageNumber,
            int pageSize)
    {
        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new ResponseModel<PagedResponse<QuestionDto>>
        {
            Success = true,
            Message = "Questions retrieved successfully.",
            Data = new PagedResponse<QuestionDto>
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
}

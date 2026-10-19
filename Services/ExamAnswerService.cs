
using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;

namespace UniversityAcademicApi.Services;

public class ExamAnswerService(
    IExamAnswerRepository answerRepository,
    IQuestionRepository questionRepository,
    ILogger<ExamAnswerService> logger)
    : IExamAnswerService
{
    public async Task<ResponseModel<ExamAnswerDto>> CreateExamAnswerAsync(
    ExamAnswerCreateDto create)
    {
        logger.LogInformation(
            "Submitting answer for attempt {AttemptId}, question {QuestionId}",
            create.ExamAttemptId, create.QuestionId);

        var attempt = await answerRepository.GetExamAttemptByIdAsync(
            create.ExamAttemptId);

        if (attempt is null)
            throw new NotFoundException("Exam attempt not found.");

        if (attempt.IsSubmitted)
        {
            throw new ConflictException(
                "You cannot submit answers because this exam attempt has already been submitted.");
        }


        var question = await questionRepository.GetQuestionByIdAsync(
            create.QuestionId);

        if (question is null)
            throw new NotFoundException("Question not found.");

        if (question.CourseId != attempt.CourseId)
        {
            throw new ConflictException(
                "This question does not belong to the course being attempted.");
        }

        if (await answerRepository.AnswerExistsAsync(
            create.ExamAttemptId, create.QuestionId))
        {
            throw new ConflictException(
                "An answer for this question already exists in this attempt.");
        }

        var selectedAnswer = create.SelectedAnswer
            .Trim()
            .ToUpperInvariant();

        var answer = new ExamAnswer
        {
            ExamAttemptId = create.ExamAttemptId,
            QuestionId = create.QuestionId,
            SelectedAnswer = selectedAnswer,
            IsCorrect = selectedAnswer == question.CorrectAnswer
                .Trim().ToUpperInvariant()
        };

        await answerRepository.CreateExamAnswerAsync(answer);

        logger.LogInformation(
            "Answer {AnswerId} saved successfully", answer.Id);

        return new ResponseModel<ExamAnswerDto>
        {
            Success = true,
            Message = "Answer picked successfully.",
            Data = new ExamAnswerDto
            {
                Id = answer.Id,
                ExamAttemptId = answer.ExamAttemptId,
                QuestionId = answer.QuestionId,
                SelectedAnswer = answer.SelectedAnswer
            }
        };
    }

    public async Task<ResponseModel<ExamAnswerDto?>> GetExamAnswerByIdAsync(
        Guid id)
    {
        var answer = await answerRepository.GetExamAnswerByIdAsync(id);

        if (answer is null)
            throw new NotFoundException("Exam answer not found.");

        return new ResponseModel<ExamAnswerDto?>
        {
            Success = true,
            Message = "Exam answer retrieved successfully.",
            Data = MapToDto(answer)
        };
    }

    public async Task<ResponseModel<IEnumerable<ExamAnswerDto>>>
        GetAnswersByAttemptAsync(Guid attemptId)
    {
        var attempt = await answerRepository.GetExamAttemptByIdAsync(
            attemptId);

        if (attempt is null)
            throw new NotFoundException("Exam attempt not found.");

        var answers = await answerRepository.GetAnswersByAttemptAsync(
            attemptId);

        return new ResponseModel<IEnumerable<ExamAnswerDto>>
        {
            Success = true,
            Message = "Exam answers retrieved successfully.",
            Data = answers.Select(MapToDto).ToList()
        };
    }

    public async Task<ResponseModel<bool>> UpdateExamAnswerAsync(
        Guid id, ExamAnswerCreateDto update)
    {
        var answer = await answerRepository.GetExamAnswerByIdAsync(id);

        if (answer is null)
            throw new NotFoundException("Exam answer not found.");

        if (update.ExamAttemptId != answer.ExamAttemptId)
        {
            throw new ConflictException(
                "An answer cannot be moved to another exam attempt.");
        }


        var attempt = await answerRepository.GetExamAttemptByIdAsync(
            answer.ExamAttemptId);

        if (attempt is null)
            throw new NotFoundException("Exam attempt not found.");

        if (attempt.IsSubmitted)
        {
            throw new ConflictException(
                "You cannot update answers because this exam attempt has already been submitted.");
        }


        var question = await questionRepository.GetQuestionByIdAsync(
            update.QuestionId);

        if (question is null)
            throw new NotFoundException("Question not found.");

        if (question.CourseId != attempt.CourseId)
        {
            throw new ConflictException(
                "This question does not belong to the course being attempted.");
        }

        if (update.QuestionId != answer.QuestionId &&
            await answerRepository.AnswerExistsAsync(
                answer.ExamAttemptId, update.QuestionId))
        {
            throw new ConflictException(
                "An answer for this question already exists in this attempt.");
        }

        var selectedAnswer = update.SelectedAnswer
            .Trim()
            .ToUpperInvariant();

        answer.QuestionId = update.QuestionId;
        answer.SelectedAnswer = selectedAnswer;
        answer.IsCorrect = selectedAnswer == question.CorrectAnswer
            .Trim().ToUpperInvariant();

        var updated = await answerRepository.UpdateExamAnswerAsync(answer);

        if (!updated)
            throw new InvalidOperationException("Failed to update exam answer.");

        return new ResponseModel<bool>
        {
            Success = true,
            Message = "Exam answer updated successfully.",
            Data = true
        };
    }

    public async Task<ResponseModel<bool>> DeleteExamAnswerAsync(Guid id)
    {
        var answer = await answerRepository.GetExamAnswerByIdAsync(id);

        if (answer is null)
            throw new NotFoundException("Exam answer not found.");

        var deleted = await answerRepository.DeleteExamAnswerAsync(answer);

        if (!deleted)
            throw new InvalidOperationException("Failed to delete exam answer.");

        return new ResponseModel<bool>
        {
            Success = true,
            Message = "Exam answer deleted successfully.",
            Data = true
        };
    }

    private static ExamAnswerDto MapToDto(ExamAnswer answer)
    {
        return new ExamAnswerDto
        {
            Id = answer.Id,
            ExamAttemptId = answer.ExamAttemptId,
            QuestionId = answer.QuestionId,
            QuestionText = answer.Question.QuestionText,
            SelectedAnswer = answer.SelectedAnswer,
            //IsCorrect = answer.IsCorrect
        };
    }
}

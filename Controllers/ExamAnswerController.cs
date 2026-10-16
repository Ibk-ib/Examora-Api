
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralLimit")]
public class ExamAnswersController(
    IExamAnswerService answerService,
    IExamAttemptService examAttemptService) : ControllerBase
{
    [Authorize(Roles = "Admin,Student")]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateExamAnswer(
        [FromBody] ExamAnswerCreateDto create)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var attempt =
                await examAttemptService.GetExamAttemptByIdAsync(
                    create.ExamAttemptId);

            if (attempt.Data?.StudentId != studentId)
                return Forbid();
        }

        var result = await answerService.CreateExamAnswerAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<ExamAnswerDto?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamAnswerById(Guid id)
    {
        var result = await answerService.GetExamAnswerByIdAsync(id);

        if (!User.IsInRole("Admin") && result.Data is not null)
        {
            var studentIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var attempt =
                await examAttemptService.GetExamAttemptByIdAsync(
                    result.Data.ExamAttemptId);

            if (attempt.Data?.StudentId != studentId)
                return Forbid();
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet("by-attempt/{attemptId:guid}")]
    [ProducesResponseType(typeof(ResponseModel<IEnumerable<ExamAnswerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnswersByAttempt(Guid attemptId)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var attempt =
                await examAttemptService.GetExamAttemptByIdAsync(attemptId);

            if (attempt.Data?.StudentId != studentId)
                return Forbid();
        }

        var result = await answerService.GetAnswersByAttemptAsync(attemptId);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateExamAnswer(
        Guid id,
        [FromBody] ExamAnswerCreateDto update)
    {
        var result = await answerService.UpdateExamAnswerAsync(id, update);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExamAnswer(Guid id)
    {
        var result = await answerService.DeleteExamAnswerAsync(id);

        return Ok(result);
    }
}

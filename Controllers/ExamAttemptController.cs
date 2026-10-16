
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[Authorize(Roles = "Admin,Student")]
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralLimit")]
public class ExamAttemptsController(
    IExamAttemptService examAttemptService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateExamAttempt(
        [FromBody] ExamAttemptCreateDto create)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            create.StudentId = studentId;
        }

        var result =
            await examAttemptService.CreateExamAttemptAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<ExamAttemptDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllExamAttempts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid pagination parameters",
                Detail = "Page number must be at least 1 and page size must be between 1 and 100."
            });
        }

        var result =
            await examAttemptService.GetAllExamAttemptsAsync(
                pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<ExamAttemptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamAttemptById(Guid id)
    {
        var result =
            await examAttemptService.GetExamAttemptByIdAsync(id);

        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            if (result.Data?.StudentId != studentId)
                return Forbid();
        }

        return Ok(result);
    }

    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(typeof(ResponseModel<IEnumerable<ExamAttemptDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamAttemptsByStudent(Guid studentId)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var authenticatedStudentId))
                return Forbid();

            if (studentId != authenticatedStudentId)
                return Forbid();
        }

        var result =
            await examAttemptService.GetExamAttemptsByStudentAsync(studentId);

        return Ok(result);
    }

    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitExamAttempt(Guid id)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var existing =
                await examAttemptService.GetExamAttemptByIdAsync(id);

            if (existing.Data?.StudentId != studentId)
                return Forbid();
        }

        var result =
            await examAttemptService.SubmitExamAttemptAsync(id);

        return Ok(result);
    }
}

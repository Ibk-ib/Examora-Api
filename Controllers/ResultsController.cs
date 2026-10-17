
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Repositories;
using UniversityAcademicApi.Services;



namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralLimit")]
public class ResultsController(IResultService resultService, IExamAttemptRepository examAttemptRepository)
    : ControllerBase
{
    [Authorize(Roles = "Admin,Student")]
    [HttpPost("generate")]
    [ProducesResponseType(typeof(ResponseModel<ResultDto>),
        StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GenerateResult(
        [FromBody] ResultCreateDto create)
    {

        if (User.IsInRole("Student"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var attempt =
                await examAttemptRepository.GetExamAttemptByIdAsync(
                    create.ExamAttemptId);

            if (attempt is null)
                return NotFound();

            if (attempt.StudentId != studentId)
                return Forbid();
        }

        var result = await resultService.GenerateResultAsync(create);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [ProducesResponseType(
        typeof(ResponseModel<PagedResponse<ResultDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllResults(
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

        var result = await resultService.GetAllResultsAsync(
            pageNumber, pageSize);

        return Ok(result);
    }


    [Authorize(Roles = "Admin,Student")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<ResultDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResultById(Guid id)
    {
        var result = await resultService.GetResultByIdAsync(id);

        if (User.IsInRole("Admin"))
            return Ok(result);

        var studentIdClaim = User.FindFirstValue(
           ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(studentIdClaim, out var studentId))
            return Forbid();

        if (result.Data?.StudentId != studentId)
            return Forbid();

        return Ok(result);
    }


    [Authorize(Roles = "Admin,Student")]
    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(
    typeof(ResponseModel<IEnumerable<ResultDto>>),
    StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
    StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
    StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResultsByStudent(Guid studentId)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var authenticatedStudentId))
                return Forbid();

            if (studentId != authenticatedStudentId)
                return Forbid();
        }

        var result = await resultService.GetResultsByStudentAsync(studentId);

        return Ok(result);
    }
}
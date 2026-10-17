
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
public class EnrollmentsController(
    IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>),
        StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateEnrollmentAsync(
        [FromBody] EnrollmentCreateDto create)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            create.StudentId = studentId;
        }

        var result =
            await enrollmentService.CreateEnrollmentAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [ProducesResponseType(
        typeof(ResponseModel<PagedResponse<EnrollmentDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllEnrollmentsAsync(
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
            await enrollmentService.GetAllEnrollmentsAsync(
                pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<EnrollmentDto?>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEnrollmentById(Guid id)
    {
        var result =
            await enrollmentService.GetEnrollmentByIdAsync(id);

        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            if (result.Data?.StudentId != studentId)
                return Forbid();
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateEnrollment(
        Guid id,
        [FromBody] EnrollmentCreateDto update)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var existing =
                await enrollmentService.GetEnrollmentByIdAsync(id);

            if (existing.Data?.StudentId != studentId)
                return Forbid();

            update.StudentId = studentId;
        }

        var result =
            await enrollmentService.UpdateEnrollmentAsync(id, update);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEnrollment(Guid id)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            var existing =
                await enrollmentService.GetEnrollmentByIdAsync(id);

            if (existing.Data?.StudentId != studentId)
                return Forbid();
        }

        var result =
            await enrollmentService.DeleteEnrollmentAsync(id);

        return Ok(result);
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[Authorize(Roles = "Admin,Student")]
[ApiController]
[Route("api/[controller]")]
public class ExamAttemptsController(
    IExamAttemptService examAttemptService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]

    public async Task<IActionResult> CreateExamAttempt(
        [FromBody] ExamAttemptCreateDto create)
    {
        var result =
            await examAttemptService.CreateExamAttemptAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<EnrollmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<CourseDto>>), StatusCodes.Status400BadRequest)]
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetExamAttemptById(Guid id)
    {
        var result =
            await examAttemptService.GetExamAttemptByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamAttemptsByStudent(
        Guid studentId)
    {
        var result =
            await examAttemptService.GetExamAttemptsByStudentAsync(
                studentId);

        return Ok(result);
    }

    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitExamAttempt(Guid id)
    {
        var result =
            await examAttemptService.SubmitExamAttemptAsync(id);

        return Ok(result);
    }
}

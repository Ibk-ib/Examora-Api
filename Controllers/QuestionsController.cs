
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionService questionService)
    : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateQuestion(
        [FromBody] QuestionCreateDto create)
    {
        var result = await questionService.CreateQuestionAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet]
    [ProducesResponseType(
        typeof(ResponseModel<PagedResponse<QuestionDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllQuestions(
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

        var result = await questionService.GetAllQuestionsAsync(
            pageNumber, pageSize);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet("by-course/{courseId:guid}")]
    [ProducesResponseType(
        typeof(ResponseModel<PagedResponse<QuestionDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionsByCourse(
        Guid courseId,
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

        var result = await questionService.GetQuestionsByCourseAsync(
            courseId, pageNumber, pageSize);

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<QuestionDto?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionById(Guid id)
    {
        var result = await questionService.GetQuestionByIdAsync(id);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateQuestion(
        Guid id,
        [FromBody] QuestionCreateDto update)
    {
        var result = await questionService.UpdateQuestionAsync(id, update);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var result = await questionService.DeleteQuestionAsync(id);

        return Ok(result);
    }
}

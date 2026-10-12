
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamAnswersController(IExamAnswerService answerService)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateExamAnswer(
        [FromBody] ExamAnswerCreateDto create)
    {
        var result = await answerService.CreateExamAnswerAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(ResponseModel<ExamAnswerDto?>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExamAnswerById(Guid id)
    {
        var result = await answerService.GetExamAnswerByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("by-attempt/{attemptId:guid}")]
    [ProducesResponseType(
        typeof(ResponseModel<IEnumerable<ExamAnswerDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAnswersByAttempt(Guid attemptId)
    {
        var result = await answerService.GetAnswersByAttemptAsync(attemptId);

        return Ok(result);
    }

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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExamAnswer(Guid id)
    {
        var result = await answerService.DeleteExamAnswerAsync(id);

        return Ok(result);
    }
}

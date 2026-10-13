
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController(IResultService resultService)
    : ControllerBase
{
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
        var result = await resultService.GenerateResultAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<ResultDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResultById(Guid id)
    {
        var result = await resultService.GetResultByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(
        typeof(ResponseModel<IEnumerable<ResultDto>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetResultsByStudent(Guid studentId)
    {
        var result =
            await resultService.GetResultsByStudentAsync(studentId);

        return Ok(result);
    }
}

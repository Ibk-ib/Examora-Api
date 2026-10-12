
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionService questionService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateQuestion(
        [FromBody] QuestionCreateDto create)
    {
        var result = await questionService.CreateQuestionAsync(create);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuestions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination values.");

        var result = await questionService.GetAllQuestionsAsync(
            pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("by-course/{courseId:guid}")]
    public async Task<IActionResult> GetQuestionsByCourse(
        Guid courseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination values.");

        var result = await questionService.GetQuestionsByCourseAsync(
            courseId, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetQuestionById(Guid id)
    {
        var result = await questionService.GetQuestionByIdAsync(id);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateQuestion(
        Guid id,
        [FromBody] QuestionCreateDto update)
    {
        var result = await questionService.UpdateQuestionAsync(id, update);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var result = await questionService.DeleteQuestionAsync(id);

        return Ok(result);
    }
}

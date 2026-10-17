using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("GeneralLimit")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]

    public async Task<IActionResult> CreateCourseAsync([FromBody] CourseCreateDto dto) {

        var result = await courseService.CreateCourseAsync(dto);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]

    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> UpdateCourseAsync([FromRoute] Guid id, [FromBody] CourseCreateDto dto) {
        
        var result = await courseService.UpdateCourseAsync(id, dto);
        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
    {
        var result = await courseService.DeleteCourseAsync(id);
        return Ok(result); 
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<CourseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<CourseDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCourses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest("Page number must be greater than 0.");
        }
        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Page size must be between 1 and 100.");
        }
        var result = await courseService.GetAllCoursesAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById([FromRoute] Guid id)
    {
        var result = await courseService.GetCourseByIdAsync(id);
        return Ok(result);
    }

} 
        

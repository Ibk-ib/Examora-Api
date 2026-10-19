using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;
using System.Security.Claims;

namespace UniversityAcademicApi.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("GeneralLimit")]
public class StudentsController(IStudentService studentService) : ControllerBase
{


    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStudentAsync([FromBody] StudentCreateDto dto)
    {
        var result = await studentService.CreateStudentAsync(dto);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Student")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateStudent(
    [FromRoute] Guid id,    
    [FromBody] StudentUpdateDto request)
    {
        if (!User.IsInRole("Admin"))
        {
            var studentIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Forbid();

            if (studentId != id)
                return Forbid();
        }

        var result = await studentService.UpdateStudentAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
    {
        var result = await studentService.DeleteStudentAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<StudentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<PagedResponse<StudentDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllStudents(
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


        var result = await studentService.GetAllStudentsAsync(pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("get-by-matric-number")]
    [ProducesResponseType(typeof(ResponseModel<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<StudentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<StudentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStudentByMatricNumber([FromQuery] string matricNumber)
    {
        var result = await studentService.GetStudentByMatricNumberAsync(matricNumber);
        return Ok(result);
    }
}
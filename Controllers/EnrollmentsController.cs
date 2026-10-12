using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]

        public async Task<IActionResult> CreateEnrollmentAsync([FromBody] EnrollmentCreateDto create)
        {

            var result = await enrollmentService.CreateEnrollmentAsync(create);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<PagedResponse<EnrollmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<PagedResponse<CourseDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllEnrollmentsAsync(
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page number must be at least 1 and page size must be between 1 and 100.");
            };

            var result = await enrollmentService.GetAllEnrollmentsAsync(
            pageNumber, pageSize);

            return Ok(result);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<CourseDto>), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetEnrollmentById(Guid id)
        {
            var result = await enrollmentService.GetEnrollmentByIdAsync(id);

            return Ok(result);
        }

        [HttpPut("{id}")]

        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> UpdateEnrollment(
        Guid id,
        [FromBody] EnrollmentCreateDto update)
        {
            var result = await enrollmentService.UpdateEnrollmentAsync(id, update);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteEnrollment(Guid id)
        {
            var result = await enrollmentService.DeleteEnrollmentAsync(id);

            return Ok(result);
        }

    }
}

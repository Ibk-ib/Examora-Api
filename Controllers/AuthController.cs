using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Services;

namespace UniversityAcademicApi.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [AllowAnonymous]
        [EnableRateLimiting("LoginLimit")]
        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel<AuthResponseDto>>> Register(
            StudentRegisterDto dto)
        {
            var response = await authService.RegisterStudentAsync(dto);

            return Ok(response);
        }

        [AllowAnonymous]
        [EnableRateLimiting("LoginLimit")]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<AuthResponseDto>>> Login(
            LoginDto dto)
        {
            var response = await authService.LoginAsync(dto);

            return Ok(response);
        }
    }
}
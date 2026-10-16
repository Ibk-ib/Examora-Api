
using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;

namespace UniversityAcademicApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(
            IStudentRepository studentRepository,
            IPasswordHashingService passwordHashingService,
            IJwtTokenService jwtTokenService,
            ILogger<AuthService> logger,
            IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _passwordHashingService = passwordHashingService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ResponseModel<AuthResponseDto>> RegisterStudentAsync(
            StudentRegisterDto dto)
        {

            var matricNumber = dto.MatricNumber.Trim().ToUpperInvariant();

            if (await _studentRepository.GetStudentByMatricNumberAsync(
                matricNumber) is not null)
            {
                throw new ConflictException(
                    "An account with this matric number already exists.");
            }

            var student = new Student
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                MatricNumber = dto.MatricNumber,
                Department = dto.Department.Trim(),
                Level = dto.Level,
                Role = "Student",
                PasswordHash = _passwordHashingService.HashPassword(
                    dto.Password)
            };

            var created = await _studentRepository.CreateStudentAsync(student);

            if (!created)
            {
                throw new InvalidOperationException(
                    "The student account could not be created.");
            }

            _logger.LogInformation(
                "Student account created for {StudentId}",
                student.Id);

            return new ResponseModel<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = new AuthResponseDto
                {
                    AccessToken = _jwtTokenService.GenerateToken(student),
                    ExpiresAtUtc = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:ExpirationMinutes")),
                    StudentId = student.Id,
                    Email = student.Email,
                    Role = student.Role
                },
                Errors = new List<ApiError>()
            };
        }

        public async Task<ResponseModel<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var student = await _studentRepository.GetStudentByMatricNumberAsync(
                dto.MatricNumber.Trim().ToUpperInvariant());

            if (student is null ||
                !_passwordHashingService.VerifyPassword(
                    dto.Password, student.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                     "Invalid matric number or password.");
            }

            return new ResponseModel<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = new AuthResponseDto
                {
                    AccessToken = _jwtTokenService.GenerateToken(student),
                    ExpiresAtUtc = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:ExpirationMinutes")),
                    StudentId = student.Id,
                    Email = student.Email,
                    Role = student.Role
                },
                Errors = new List<ApiError>()
            };
        }
    }
}

using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
namespace UniversityAcademicApi.Services;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;

public class CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger) : ICourseService
{
    public async Task<ResponseModel<bool>> CreateCourseAsync(CourseCreateDto create)
    {
        logger.LogInformation("Creating a new course with code: {CourseCode}", create.CourseCode);

        var courseExists = await courseRepository.CourseCodeExistsAsync(create.CourseCode, create.CourseTitle);

        if (courseExists)
        {
            logger.LogWarning("A course with code {CourseCode} or title {CourseTitle} already exists.", create.CourseCode, create.CourseTitle);
            throw new ConflictException("A course with the same code or title already exists.");
        }

        var course = new Course
        {
            CourseCode = create.CourseCode,
            CourseTitle = create.CourseTitle,
            Description = create.Description,
            CreditUnit = create.CreditUnit,
            Department = create.Department,
            Level = create.Level
        };

        var result = await courseRepository.CreateCourseAsync(course);

        return new ResponseModel<bool>
        {
            Success = result,
            Message = result ? "Course created successfully." : "Failed to create course.",
            Data = result
        };
    }

    public async Task<ResponseModel<bool>> DeleteCourseAsync(Guid id) { 
    
        var course = await courseRepository.GetCourseByIdAsync(id);
        if (course == null) {
            throw new NotFoundException("Course not found.");
        }

        var related = await courseRepository.HasRelatedRecordsAsync(id);
        if (related) {
            throw new ConflictException("This course cannot be deleted because it has related records.");
        }

        var result = await courseRepository.DeleteCourseAsync(course);

        return new ResponseModel<bool>
        {
            Success = result,
            Message = result ? "Course deleted successfully." : "Failed to delete course.",
            Data = result
        };
    }

    public async Task<ResponseModel<PagedResponse<CourseDto>>> GetAllCoursesAsync(int pageNumber, int pageSize) {
        var result = await courseRepository.GetAllCoursesAsync(pageNumber, pageSize);

        var courseDtos = result.Courses.Select(c => new CourseDto
        {
            Id = c.Id,
            CourseCode = c.CourseCode,
            CourseTitle = c.CourseTitle,
            Description = c.Description,
            CreditUnit = c.CreditUnit,
            Department = c.Department,
            Level = c.Level
        }).ToList();

        var totalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);

        var pageResponse = new PagedResponse<CourseDto>
        {
          Items = courseDtos,
          PageNumber = pageNumber,
          PageSize = pageSize,
          TotalCount = result.TotalCount,
          TotalPages = totalPages,
          HasNextPage = pageNumber < totalPages,
          HasPreviousPage = pageNumber > 1
        };

        return  new ResponseModel<PagedResponse<CourseDto>>
        {
            Success = true,
            Message = "Courses retrieved successfully.",
            Data = pageResponse
        };
    }

    public async Task<ResponseModel<CourseDto?>> GetCourseByIdAsync(Guid id) {
        var course = await courseRepository.GetCourseByIdAsync(id);
        
        if (course == null)
        {
            throw new NotFoundException("Course not found.");
        }
        var courseDto = new CourseDto
        {

            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseTitle = course.CourseTitle,
            Description = course.Description,
            CreditUnit = course.CreditUnit,
            Department = course.Department,
            Level = course.Level
        };

        return new ResponseModel<CourseDto?>
        {
            Success = true,
            Message = "Course retrieved successfully.",
            Data = courseDto
        };

    }


        public async Task<ResponseModel<bool>> UpdateCourseAsync(
            Guid id,
            CourseCreateDto request)
        {
            var course = await courseRepository.GetCourseByIdAsync(id);

            if (course == null)
            {
                throw new NotFoundException("Course not found.");
            }

            var courseCodeExists =
                await courseRepository.CourseCodeExistsAsync(
                    request.CourseCode,request.CourseTitle,
                    id);

            if (courseCodeExists)
            {
                throw new ConflictException(
                    "Another course already uses the provided course code or title.");
            }

            course.CourseCode = request.CourseCode;
            course.CourseTitle = request.CourseTitle;
            course.Description = request.Description;
            course.CreditUnit = request.CreditUnit;
            course.Department = request.Department;
            course.Level = request.Level;

            var result = await courseRepository.UpdateCourseAsync(course);

            return new ResponseModel<bool>
            {
                Success = result,
                Message = result
                    ? "Course updated successfully."
                    : "Failed to update course.",
                Data = result
            };
        }
}

using UniversityAcademicApi.DTOs;
namespace UniversityAcademicApi.Services;

 public interface ICourseService 
 {
    public Task<ResponseModel<bool>> CreateCourseAsync(CourseCreateDto create);
    public Task<ResponseModel<bool>> DeleteCourseAsync(Guid id);
    public Task<ResponseModel<PagedResponse<CourseDto>>> GetAllCoursesAsync(int pageNumber, int pageSize);
    public Task<ResponseModel<CourseDto?>> GetCourseByIdAsync(Guid id);
    public Task<ResponseModel<bool>> UpdateCourseAsync(Guid id, CourseCreateDto update);
}


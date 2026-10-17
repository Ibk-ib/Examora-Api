using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using UniversityAcademicApi.Data;
using UniversityAcademicApi.DTOs;
using UniversityAcademicApi.Exceptions;
using UniversityAcademicApi.Repositories;


namespace UniversityAcademicApi.Services
{
    public class EnrollmentService(IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository,
    ICourseRepository courseRepository, ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task<ResponseModel<bool>> CreateEnrollmentAsync(EnrollmentCreateDto create) {

            logger.LogInformation("Creating enrollment for {StudentId} in {CourseId}", create.StudentId, create.CourseId);
             
            var studentExist = await studentRepository.GetStudentByIdAsync(create.StudentId);

            if (studentExist is null) { 
                logger.LogWarning("Student with ID {StudentId} not found", create.StudentId);
                throw new NotFoundException("Student not found.");
            }

            var courseExist = await courseRepository.GetCourseByIdAsync(create.CourseId);

            if (courseExist is null) {

                logger.LogWarning("Course with ID {CourseId} not found", create.CourseId);
                throw new NotFoundException("Course not found." );
            }

            var enrollmentExist = await enrollmentRepository.EnrollmentExistAsync(create.StudentId, create.CourseId);

            if (enrollmentExist)
            {
                logger.LogWarning("Enrollment already exists for StudentId {StudentId} and CourseId {CourseId}", create.StudentId, create.CourseId);
                throw new ConflictException("Enrollment already exists for this student and course.");
            }

            var enrollment = new Enrollment
            {

                CourseId = create.CourseId,
                StudentId = create.StudentId,
                EnrollmentDate = DateTime.UtcNow
            };

            var created = await enrollmentRepository.CreateEnrollmentAsync(enrollment);
            logger.LogInformation("Enrollment created successfully for StudentId {StudentId} in CourseId {CourseId}", create.StudentId, create.CourseId);

            if (!created)
            {
                logger.LogError("Failed to create enrollment for StudentId {StudentId} in CourseId {CourseId}", create.StudentId, create.CourseId);
                throw new InvalidOperationException("Failed to create enrollment.");
            }

            return new ResponseModel<bool>
            {
                Success = true,
                Message = "Enrollment created successfully.",
                Data = true
            };
        }

        public async Task<ResponseModel<bool>> UpdateEnrollmentAsync(Guid id, EnrollmentCreateDto update) {

            var enrollment =
            await enrollmentRepository.GetEnrollmentByIdAsync(id);

            if (enrollment is null)
            {
                throw new NotFoundException("Enrollment not found.");
            }

            var student = await studentRepository.GetStudentByIdAsync(
                update.StudentId);

            if (student is null)
            {
                throw new NotFoundException("Student not found.");
            }

            var course = await courseRepository.GetCourseByIdAsync(
                update.CourseId);

            if (course is null)
            {
                throw new NotFoundException("Course not found.");
            }

            var alreadyEnrolled =
                await enrollmentRepository.EnrollmentExistAsync(
                    update.StudentId,
                    update.CourseId,
                    id);

            if (alreadyEnrolled)
            {
                throw new ConflictException(
                    "This student is already enrolled in this course.");
            }

            enrollment.StudentId = update.StudentId;
            enrollment.CourseId = update.CourseId;

            var updated = await enrollmentRepository
                .UpdateEnrollmentAsync(enrollment);

            if (!updated)
            {
                throw new InvalidOperationException(
                    "Failed to update enrollment.");
            }

            logger.LogInformation(
                "Enrollment {EnrollmentId} updated successfully",
                id);

            return new ResponseModel<bool>
            {
                Success = true,
                Message = "Enrollment updated successfully.",
                Data = true
            };
        }

        public async Task<ResponseModel<bool>> DeleteEnrollmentAsync(Guid id) {

            var enrollment = await enrollmentRepository.GetEnrollmentByIdAsync(id);

            if (enrollment is null) {

                throw new NotFoundException("Enrollment not found.");

            }

            var deleted = await enrollmentRepository.DeleteEnrollmentAsync(enrollment);

            //if(!deleted)
            //{
            //    throw new InvalidOperationException("Failed to delete enrollment.");
            //}

            logger.LogInformation(
                "Enrollment {EnrollmentId} deleted successfully",
                id);

            return new ResponseModel<bool>
            {
                Success = deleted,
                Message = deleted? "Enrollment deleted successfully." : "Failed to delete enrollment.",
                Data = deleted
            };

        }

        public async Task<ResponseModel<PagedResponse<EnrollmentDto>>> GetAllEnrollmentsAsync(int pageNumber, int pageSize) { 
            
            var result = await enrollmentRepository.GetAllEnrollmentsAsync(pageNumber, pageSize);

            var items = result.Enrollments.Select(e => new EnrollmentDto
            {
                
                Id = e.Id,
                StudentId = e.StudentId,  
                CourseId = e.CourseId,
                StudentName = $"{e.Student.FirstName} {e.Student.LastName}", 
                CourseCode = e.Course.CourseCode,
                CourseTitle = e.Course.CourseTitle,
                EnrollmentDate = e.EnrollmentDate 
            }).ToList(); 

            var totalPages = (int)Math.Ceiling ((double)result.TotalCount / pageSize);

            var data = new PagedResponse<EnrollmentDto>
            {

                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };
            return new ResponseModel<PagedResponse<EnrollmentDto>>
            {
                Success = true,
                Message = "Enrollments retrieved successfully.", 
                Data = data
            };
        } 

        public async Task<ResponseModel<EnrollmentDto?>> GetEnrollmentByIdAsync(Guid id) { 
            
            var result = await enrollmentRepository.GetEnrollmentByIdAsync(id);

            if (result is null) {

                throw new NotFoundException("Enrollment not found.");

            }
            var dto = new EnrollmentDto
            {
                Id = result.Id,
                StudentId = result.StudentId,
                StudentName =
                $"{result.Student.FirstName} {result.Student.LastName}",
                CourseId = result.CourseId,
                CourseCode = result.Course.CourseCode,
                CourseTitle = result.Course.CourseTitle,
                EnrollmentDate = result .EnrollmentDate
            };
            return new ResponseModel<EnrollmentDto?>
            {
                Success = true,
                Message = "Enrollment retrieved successfully.",
                Data = dto
            };

        }


    }
}

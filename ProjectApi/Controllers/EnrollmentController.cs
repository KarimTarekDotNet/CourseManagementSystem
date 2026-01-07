using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Entities.Enums;
using Project.Queries;
using Project.Service;
using ProjectApi.DTOs;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;
    private readonly AppDbContext context;

    public EnrollmentController(EnrollmentService enrollmentService, AppDbContext context)
    {
        _enrollmentService = enrollmentService;
        this.context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnrollment(int studentId, int courseId, DateTime startDate, EnrollmentStatus status)
    {
        try
        {
            var enrollment = await _enrollmentService.CreateEnrollment(studentId, courseId, startDate, status);

            return Ok(new EnrollmentDTO
            {
                Id = enrollment.Id,
                StartEnrollmentDate = enrollment.StartEnrollmentDate,
                EndEnrollmentDate = enrollment.EndEnrollmentDate,
                Status = enrollment.Status,
                CourseName = enrollment.Course.Name,
                StudentName = enrollment.Student.FullName
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DropEnrollment(int studentId, int courseId)
    {
        try
        {
            await _enrollmentService.DropEnrollment(studentId, courseId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEnrollments()
    {
        try
        {
            var enrollments = EnrollmentQueries.GetAllEnrollment(context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable());

            var enrollmentDTOs = await enrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            }).ToListAsync();

            return Ok(enrollmentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{studentId}/student")]
    public async Task<IActionResult> GetStudentEnrollments(int studentId)
    {
        try
        {
            var student = await context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found");

            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var studentEnrollments = EnrollmentQueries.GetStudentEnrollment(enrollmentsQuery, student);

            var enrollmentDTOs = await studentEnrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            }).ToListAsync();

            return Ok(enrollmentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{courseId}/course")]
    public async Task<IActionResult> GetCourseEnrollments(int courseId)
    {
        try
        {
            var course = await context.Courses.FindAsync(courseId);
            if (course == null) return NotFound("Course not found");

            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var courseEnrollments = EnrollmentQueries.GetCourseEnrollment(enrollmentsQuery, course);

            var enrollmentDTOs = await courseEnrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            }).ToListAsync();

            return Ok(enrollmentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{studentId}/{courseId}/check")]
    public async Task<IActionResult> CheckStudentEnrollment(int studentId, int courseId)
    {
        try
        {
            var student = await context.Students.FindAsync(studentId);
            var course = await context.Courses.FindAsync(courseId);

            if (student == null || course == null)
                return NotFound("Student or Course not found");

            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var exists = EnrollmentQueries.GetStudentIfAlreadyEnrolled(enrollmentsQuery, student, course).Any();

            return Ok(new { IsEnrolled = exists });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{studentId}/count-courses")]
    public async Task<IActionResult> CountStudentCourses(int studentId)
    {
        try
        {
            var student = await context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found");

            var enrollmentsQuery = context.Enrollments.AsQueryable();
            var count = EnrollmentQueries.CountCoursesForStudent(enrollmentsQuery, student);

            return Ok(new { CoursesCount = count });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{studentId}/most-enrolled-course")]
    public async Task<IActionResult> GetMostEnrolledCourseForStudent(int studentId)
    {
        try
        {
            var student = await context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Student not found");

            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var mostEnrolledCourse = EnrollmentQueries.GetStudentMostEnrolledCourse(enrollmentsQuery, student);

            if (mostEnrolledCourse == null) return NotFound("No courses found for this student");

            return Ok(new { CourseId = mostEnrolledCourse.Id, CourseName = mostEnrolledCourse.Name });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{courseId}/empty")]
    public async Task<IActionResult> IsCourseEmpty(int courseId)
    {
        try
        {
            var course = await context.Courses.FindAsync(courseId);
            if (course == null) return NotFound("Course not found");

            var enrollmentsQuery = context.Enrollments.AsQueryable();
            var isEmpty = EnrollmentQueries.EmptyCourse(enrollmentsQuery, course);

            return Ok(new { IsEmpty = isEmpty });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{courseId}/last-week")]
    public async Task<IActionResult> GetLastWeekEnrollments(int courseId)
    {
        try
        {
            var course = await context.Courses.FindAsync(courseId);
            if (course == null) return NotFound("Course not found");

            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var lastWeekEnrollments = EnrollmentQueries.LastEnrollment(enrollmentsQuery, course);

            var enrollmentDTOs = await lastWeekEnrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            }).ToListAsync();

            return Ok(enrollmentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetEnrollmentsByStatus(EnrollmentStatus status)
    {
        try
        {
            var enrollmentsQuery = context.Enrollments.Include(e => e.Course).Include(e => e.Student).AsQueryable();
            var filteredEnrollments = EnrollmentQueries.GetEnrollmentsByStatus(enrollmentsQuery, status);

            var enrollmentDTOs = await filteredEnrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            }).ToListAsync();

            return Ok(enrollmentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

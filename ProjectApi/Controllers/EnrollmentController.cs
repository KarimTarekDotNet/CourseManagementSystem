using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Entities.Enums;
using Project.Queries;
using Project.Service;
using ProjectApi.DTOs;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;
    private readonly AppDbContext context;

    public EnrollmentController(EnrollmentService enrollmentService, AppDbContext context)
    {
        _enrollmentService = enrollmentService;
        this.context = context;
    }

    // POST: api/enrollment
    [HttpPost]
    public async Task<ActionResult<EnrollmentDTO>> Create([FromBody] EnrollmentCreateDTO request)
    {
        try
        {
            var enrollment = await _enrollmentService
                .CreateEnrollment(request.StudentId, request.CourseId, request.StartEnrollmentDate, request.Status);

            return Ok(MapToDTO(enrollment));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // DELETE: api/enrollment
    [HttpDelete("{studentId}/{courseId}")]
    public async Task<ActionResult> Drop(int studentId, int courseId)
    {
        await _enrollmentService.DropEnrollment(studentId, courseId);
        return NoContent();
    }

    [HttpPut("restore")]
    public async Task<IActionResult> Restore([FromBody] RestoreEnrollmentDTO dto)
    {
        await _enrollmentService.RestoreEnrollment(dto.StudentId, dto.CourseId);
        return Ok();
    }

    // GET: api/enrollment
    [HttpGet]
    public ActionResult<List<EnrollmentDTO>> GetAll()
    {
        var enrollments = EnrollmentQueries
            .GetAllEnrollments(context.Enrollments.AsQueryable());

        return Ok(MapToDTOs(enrollments));
    }

    // GET: api/enrollment/{studentId}/student
    [HttpGet("{studentId}/student")]
    public ActionResult<List<EnrollmentDTO>> GetStudentEnrollments(int studentId)
    {
        var student = context.Students.Find(studentId);
        if (student == null) return NotFound("Student not found");

        var enrollments = EnrollmentQueries.GetStudentEnrollments(
            context.Enrollments.AsQueryable(), studentId);

        return Ok(MapToDTOs(enrollments));
    }

    // GET: api/enrollment/{courseId}/course
    [HttpGet("{courseId}/course")]
    public ActionResult<List<EnrollmentDTO>> GetCourseEnrollments(int courseId)
    {
        var course = context.Courses.Find(courseId);
        if (course == null) return NotFound("Course not found");

        var enrollments = EnrollmentQueries.GetCourseEnrollments(
            context.Enrollments.AsQueryable(), courseId);

        return Ok(MapToDTOs(enrollments));
    }

    // GET: api/enrollment/{studentId}/{courseId}/check
    [HttpGet("{studentId}/{courseId}/check")]
    public async Task<ActionResult> CheckEnrollment(int studentId, int courseId)
    {
        var exists = EnrollmentQueries
            .GetStudentIfAlreadyEnrolled(context.Enrollments.AsQueryable(), studentId, courseId).Any();

        return Ok(new { IsEnrolled = exists });
    }

    // GET: api/enrollment/status/{status}
    [HttpGet("status/{status}")]
    public ActionResult<List<EnrollmentDTO>> GetByStatus(EnrollmentStatus status)
    {
        var enrollments = EnrollmentQueries.GetEnrollmentsByStatus(
            context.Enrollments.AsQueryable(), status);

        return Ok(MapToDTOs(enrollments));
    }

    // -------------------- Mapping --------------------

    private static EnrollmentDTO MapToDTO(Enrollment e) =>
        new EnrollmentDTO
        {
            Id = e.Id,
            StartEnrollmentDate = e.StartEnrollmentDate,
            EndEnrollmentDate = e.EndEnrollmentDate,
            Status = e.Status,
            CourseName = e.Course.Name,
            StudentName = e.Student.FullName
        };

    private static List<EnrollmentDTO> MapToDTOs(IQueryable<Enrollment> enrollments) =>
        enrollments
            .Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StartEnrollmentDate = e.StartEnrollmentDate,
                EndEnrollmentDate = e.EndEnrollmentDate,
                Status = e.Status,
                CourseName = e.Course.Name,
                StudentName = e.Student.FullName
            })
            .ToList();
}

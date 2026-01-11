using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Entities;
using Project.Entities.Enums;
using Project.Queries;
using Project.Service;
using ProjectApi.DTOs;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly AppDbContext _context;

        public CoursesController(CourseService courseService, AppDbContext context)
        {
            _courseService = courseService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCourses()
        {
            var coursesQuery = CourseQueries.GetAllCourses(
                _context.Courses.Include(c => c.Instructor).AsQueryable());

            var courses = await coursesQuery.ToListAsync();
            return Ok(MapToCoursesDTO(courses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound($"Course with ID {id} not found");

            return Ok(MapToCourseDTO(course));
        }

        [HttpGet("byname/{name}")]
        public async Task<ActionResult<List<CourseDTO>>> GetByName(string name)
        {
            var coursesQuery = _context.Courses
                .Include(c => c.Instructor)
                .AsQueryable()
                .Where(c => EF.Functions.Like(c.Name, $"%{name}%"));

            var courses = await coursesQuery
                .ToListAsync();

            if (!courses.Any())
                return NotFound();

            return Ok(MapToCoursesDTO(courses));
        }

        [HttpGet("WithoutInstructor")]
        public async Task<ActionResult<List<CourseDTO>>> GetWithoutInstructor(
            int pageNumber = 1, int pageSize = 10)
        {
            var coursesQuery = CourseQueries.GetCoursesWithoutInstructor(
                _context.Courses.Include(c => c.Instructor).IgnoreQueryFilters().AsQueryable());

            var courses = await coursesQuery.ToListAsync();
            return Ok(MapToCoursesDTO(courses));
        }

        [HttpGet("WithMinStudents/{count}")]
        public async Task<ActionResult<List<CourseDTO>>> GetWithMinStudents(
            int count, int pageNumber = 1, int pageSize = 10)
        {
            var coursesQuery = CourseQueries.GetCoursesWithMinStudents(
                _context.Courses.Include(c => c.Instructor).AsQueryable(),
                count);

            var courses = await coursesQuery.ToListAsync();
            return Ok(MapToCoursesDTO(courses));
        }

        [HttpGet("WithLevel/{level}")]
        public async Task<ActionResult<List<CourseDTO>>> GetWithLevel(
            CourseLevel level, int pageNumber = 1, int pageSize = 10)
        {
            var coursesQuery = CourseQueries.GetCoursesWithLevel(
                _context.Courses.Include(c => c.Instructor).AsQueryable(),
                level);

            var courses = await coursesQuery.ToListAsync();
            return Ok(MapToCoursesDTO(courses));
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Create(CourseDTO courseDto)
        {
            var createdCourseId = await _courseService.AddCourse(
                courseDto.Name,
                courseDto.Description,
                courseDto.TotalHours,
                courseDto.SessionDuration,
                courseDto.Capacity,
                courseDto.Level
            );

            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourseId }, courseDto);
        }

        [HttpPut("{id}/name")]
        public async Task<IActionResult> UpdateName(int id, [FromBody] string name)
        {
            await _courseService.UpdateCourseName(id, name);
            return NoContent();
        }

        [HttpPut("{id}/capacity")]
        public async Task<IActionResult> UpdateCapacity(int id, [FromBody] int capacity)
        {
            await _courseService.UpdateCourseCapacity(id, capacity);
            return NoContent();
        }

        [HttpPut("{id}/totalHours")]
        public async Task<IActionResult> UpdateTotalHours(int id, [FromBody] int totalHours)
        {
            await _courseService.UpdateCourseTotalHours(id, totalHours);
            return NoContent();
        }

        [HttpPut("{id}/sessionDuration")]
        public async Task<IActionResult> UpdateSessionDuration(int id, [FromBody] int sessionDuration)
        {
            await _courseService.UpdateCourseSessionDuration(id, sessionDuration);
            return NoContent();
        }

        [HttpPut("{id}/level")]
        public async Task<IActionResult> UpdateLevel(int id, [FromBody] CourseLevel level)
        {
            await _courseService.UpdateCourseLevel(id, level);
            return NoContent();
        }

        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] string? description)
        {
            await _courseService.UpdateCourseDescription(id, description);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteCourse(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _courseService.RestoreCourse(id);
            return NoContent();
        }

        private CourseDTO MapToCourseDTO(Course course)
        {
            return new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TotalHours = course.TotalHours,
                SessionDuration = course.SessionDuration,
                Capacity = course.Capacity,
                Level = course.Level,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor?.FullName
            };
        }

        private static List<CourseDTO> MapToCoursesDTO(IEnumerable<Course> courses)
        {
            return courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                TotalHours = c.TotalHours,
                SessionDuration = c.SessionDuration,
                Capacity = c.Capacity,
                Level = c.Level,
                InstructorId = c.InstructorId,
                InstructorName = c.Instructor?.FullName
            }).ToList();
        }
    }
}

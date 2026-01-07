using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Queries;
using Project.Service;
using ProjectApi.DTOs;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;
        private readonly AppDbContext context;

        public CoursesController(CourseService courseService, AppDbContext context)
        {
            _courseService = courseService;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCourses()
        {
            var courses = await CourseQueries.GetAllCourses(context.Courses.AsQueryable()).ToListAsync();
            var coursesDto = courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                TotalHours = c.TotalHours,
                SessionDuration = c.SessionDuration,
                Capacity = c.Capacity,
                Level = c.Level
            }).ToList();
            return Ok(coursesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetById(int id)
        {
            var courses = CourseQueries.GetAllCourses(context.Courses.AsQueryable());
            var course = await courses.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                TotalHours = course.TotalHours,
                SessionDuration = course.SessionDuration,
                Capacity = course.Capacity,
                Level = course.Level
            };
            return Ok(courseDto);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Create(CourseDTO courseDto)
        {
            await _courseService.AddCourse(courseDto.Name, courseDto.Description, courseDto.TotalHours, courseDto.SessionDuration, courseDto.Capacity, courseDto.Level);
            return CreatedAtAction(nameof(GetById), new { id = courseDto.Id }, courseDto);
        }

        [HttpPut("{id}/name")]
        public async Task<IActionResult> UpdateName(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseName(id, courseDto.Name);
            return NoContent();
        }

        [HttpPut("{id}/capacity")]
        public async Task<IActionResult> UpdateCapacity(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseCapacity(id, courseDto.Capacity);
            return NoContent();
        }

        [HttpPut("{id}/totalHours")]
        public async Task<IActionResult> UpdateTotalHours(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseTotalHours(id, courseDto.TotalHours);
            return NoContent();
        }

        [HttpPut("{id}/sessionDuration")]
        public async Task<IActionResult> UpdateSessionDuration(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseSessionDuration(id, courseDto.SessionDuration);
            return NoContent();
        }

        [HttpPut("{id}/level")]
        public async Task<IActionResult> UpdateLevel(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseLevel(id, courseDto.Level);
            return NoContent();
        }

        [HttpPut("{id}/Description")]
        public async Task<IActionResult> UpdateDescription(int id, CourseDTO courseDto)
        {
            if (id != courseDto.Id) return BadRequest();
            await _courseService.UpdateCourseDescription(id, courseDto.Description);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteCourse(id);
            return NoContent();
        }
    }
}
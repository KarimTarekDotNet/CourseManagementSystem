using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Service;
using Project.Queries;
using ProjectApi.DTOs;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly InstructorService _instructorService;
        private readonly AppDbContext context;

        public InstructorController(InstructorService instructorService, AppDbContext context)
        {
            _instructorService = instructorService;
            this.context = context;
        }

        // GET: api/instructor
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors()
        {
            try
            {
                var instructors = InstructorQueries.GetAllInstructors(context.Instructors.AsQueryable());
                var dtos = await instructors.Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Department = i.Department,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber
                }).ToListAsync();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/instructor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
        {
            try
            {
                var instructor = await context.Instructors.Include(i => i.Courses)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (instructor == null) return NotFound("Instructor not found");

                var dto = new InstructorDTO
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    Department = instructor.Department,
                    Email = instructor.Email,
                    PhoneNumber = instructor.PhoneNumber
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/instructor
        [HttpPost]
        public async Task<IActionResult> AddInstructor(InstructorDTO dto)
        {
            try
            {
                await _instructorService.AddInstructor(
                    dto.FirstName,
                    dto.LastName,
                    dto.Department,
                    dto.Email,
                    dto.PhoneNumber
                );

                return Ok("Instructor added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/instructor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveInstructor(int id)
        {
            try
            {
                await _instructorService.RemoveInstructor(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/firstname
        [HttpPut("{id}/firstname")]
        public async Task<IActionResult> UpdateFirstName(int id, InstructorDTO dto)
        {
            try
            {
                await _instructorService.UpdateInstructorFirstName(id, dto.FirstName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/lastname
        [HttpPut("{id}/lastname")]
        public async Task<IActionResult> UpdateLastName(int id, InstructorDTO dto)
        {
            try
            {
                await _instructorService.UpdateInstructorLastName(id, dto.LastName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/email
        [HttpPut("{id}/email")]
        public async Task<IActionResult> UpdateEmail(int id, InstructorDTO dto)
        {
            try
            {
                await _instructorService.UpdateInstructorEmail(id, dto.Email);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/department
        [HttpPut("{id}/department")]
        public async Task<IActionResult> UpdateDepartment(int id, InstructorDTO dto)
        {
            try
            {
                await _instructorService.UpdateInstructorDepartment(id, dto.Department);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/phone
        [HttpPut("{id}/phone")]
        public async Task<IActionResult> UpdatePhoneNumber(int id, InstructorDTO dto)
        {
            try
            {
                await _instructorService.UpdateInstructorPhoneNumber(id, dto.PhoneNumber);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/instructor/{id}/assign-course/{courseId}
        [HttpPost("{id}/assign-course/{courseId}")]
        public async Task<IActionResult> AssignCourse(int id, int courseId)
        {
            try
            {
                await _instructorService.AssignCourse(id, courseId);
                return Ok("Course assigned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/instructor/{id}/remove-course/{courseId}
        [HttpDelete("{id}/remove-course/{courseId}")]
        public async Task<IActionResult> RemoveCourse(int id, int courseId)
        {
            try
            {
                await _instructorService.RemoveCourse(id, courseId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/instructor/department/{department}
        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetInstructorsByDepartment(string department)
        {
            try
            {
                var instructorsQuery = InstructorQueries.GetInstructorsByDepartment(context.Instructors.AsQueryable(), department);
                var dtos = await instructorsQuery.Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Department = i.Department,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber
                }).ToListAsync();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/instructor/with-courses
        [HttpGet("with-courses")]
        public async Task<IActionResult> GetInstructorsWithCourses()
        {
            try
            {
                var instructorsQuery = InstructorQueries.GetInstructorsWithAnyCourses(context.Instructors.AsQueryable());
                var dtos = await instructorsQuery.Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Department = i.Department,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber
                }).ToListAsync();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/instructor/without-courses
        [HttpGet("without-courses")]
        public async Task<IActionResult> GetInstructorsWithoutCourses()
        {
            try
            {
                var instructorsQuery = InstructorQueries.GetInstructorsWithoutCourses(context.Instructors.AsQueryable());
                var dtos = await instructorsQuery.Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Department = i.Department,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber
                }).ToListAsync();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

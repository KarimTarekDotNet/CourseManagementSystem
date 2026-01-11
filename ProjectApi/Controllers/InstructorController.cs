using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Entities;
using Project.Queries;
using Project.Service;
using ProjectApi.DTOs;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public IActionResult GetAllInstructors()
        {
            var instructors = InstructorQueries
                .GetAllInstructors(context.Instructors.AsQueryable());

            var dtos = MapToInstructorsDTO(instructors);
            return Ok(dtos);
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

                var dto = MapToInstructorDTO(instructor);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/instructor
        [HttpPost]
        public async Task<ActionResult<InstructorDTO>> AddInstructor(InstructorDTO dto)
        {
            int id = await _instructorService.AddInstructor(
                dto.FirstName,
                dto.LastName,
                dto.Department,
                dto.Email,
                dto.PhoneNumber
            );

            return CreatedAtAction(nameof(GetInstructorById), new { Id = id }, dto);
        }

        // DELETE: api/instructor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveInstructor(int id)
        {
            await _instructorService.RemoveInstructor(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreInstructor(int id)
        {
            try
            {
                await _instructorService.RestoreInstructor(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/firstname
        [HttpPut("{id}/firstname")]
        public async Task<IActionResult> UpdateFirstName(int id, [FromBody] string fName)
        {
            try
            {
                await _instructorService.UpdateInstructorFirstName(id, fName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/lastname
        [HttpPut("{id}/lastname")]
        public async Task<IActionResult> UpdateLastName(int id, [FromBody] string lName)
        {
            try
            {
                await _instructorService.UpdateInstructorLastName(id, lName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/email
        [HttpPut("{id}/email")]
        public async Task<IActionResult> UpdateEmail(int id, [FromBody] string email)
        {
            try
            {
                await _instructorService.UpdateInstructorEmail(id, email);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/department
        [HttpPut("{id}/department")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] string? department)
        {
            try
            {
                await _instructorService.UpdateInstructorDepartment(id, department);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/instructor/{id}/phone
        [HttpPut("{id}/phone")]
        public async Task<IActionResult> UpdatePhoneNumber(int id, [FromBody] string phone)
        {
            try
            {
                await _instructorService.UpdateInstructorPhoneNumber(id, phone);
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
        [HttpGet("department")]
        public IActionResult GetInstructorsByDepartment(string? department)
        {
            var instructorsQuery = InstructorQueries.GetInstructorsByDepartment(context.Instructors.AsQueryable(), department);
            var dtos = MapToInstructorsDTO(instructorsQuery);

            if(!dtos.Any())
                return NotFound("No instructors found for this department");

            return Ok(dtos);
        }

        // GET: api/instructor/with-courses
        [HttpGet("with-courses")]
        public IActionResult GetInstructorsWithCourses()
        {
            try
            {
                var instructorsQuery = InstructorQueries.GetInstructorsWithAnyCourses(context.Instructors.AsQueryable());
                var dtos = MapToInstructorsDTO(instructorsQuery);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/instructor/without-courses
        [HttpGet("without-courses")]
        public IActionResult GetInstructorsWithoutCourses()
        {
            try
            {
                var instructorsQuery = InstructorQueries.GetInstructorsWithoutCourses(context.Instructors.AsQueryable());
                var dtos = MapToInstructorsDTO(instructorsQuery);

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private InstructorDTO MapToInstructorDTO(Instructor instructor)
        {
            return new InstructorDTO
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Email = instructor.Email,
                PhoneNumber = instructor.PhoneNumber,
                Department = instructor.Department,
                IsDeleted = instructor.IsDeleted,
            };
        }
        private static List<InstructorDTO> MapToInstructorsDTO(IQueryable<Instructor> instructorDTOs)
        {
            var instructors = instructorDTOs.Select(c => new InstructorDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Department = c.Department,
                IsDeleted = c.IsDeleted,
            }).ToList();
            return instructors;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly AppDbContext context;

        public StudentsController(StudentService studentService, AppDbContext context)
        {
            _studentService = studentService;
            this.context = context;
        }

        [HttpGet]
        public Task<ActionResult<List<StudentDTO>>> GetAllStudent()
        {
            var students = StudentQueries.GetAllStudents(context.Students.AsQueryable());
            var studentDTOs = MapToStudentsDTO(students);
            return Task.FromResult<ActionResult<List<StudentDTO>>>(Ok(studentDTOs));
        }

        [HttpGet("{id}")]
        public Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            var student = StudentQueries.GetStudentById(context.Students.AsQueryable(), id);
            if (student == null)
            {
                return Task.FromResult<ActionResult<StudentDTO>>(NotFound());
            }
            var studentDTO = MapToStudentDTO(student);
            return Task.FromResult<ActionResult<StudentDTO>>(Ok(studentDTO));
        }

        [HttpGet("{fName}/{lName}/name")]
        public Task<ActionResult<List<StudentDTO>>> GetStudentByName(string fName, string lName)
        {
            var students = StudentQueries.GetStudentWithName(context.Students.AsQueryable(), fName, lName);
            if (students == null)
            {
                return Task.FromResult<ActionResult<List<StudentDTO>>>(NotFound());
            }
            var studentDTOs = MapToStudentsDTO(students);
            return Task.FromResult<ActionResult<List<StudentDTO>>>(Ok(studentDTOs));
        }

        [HttpGet("{email}/email")]
        public Task<ActionResult<StudentDTO>> GetStudentByEmail(string email)
        {
            var student = StudentQueries.GetStudentWithEmail(context.Students.AsQueryable(), email);
            if (student == null)
            {
                return Task.FromResult<ActionResult<StudentDTO>>(NotFound());
            }
            var studentDTOs = MapToStudentDTO(student);
            return Task.FromResult<ActionResult<StudentDTO>>(Ok(studentDTOs));
        }

        [HttpGet("{phone}/phone")]
        public Task<ActionResult<StudentDTO>> GetStudentByPhone(string phone)
        {
            var student = StudentQueries.GetStudentWithPhone(context.Students.AsQueryable(), phone);
            if (student == null)
            {
                return Task.FromResult<ActionResult<StudentDTO>>(NotFound());
            }
            var studentDTOs = MapToStudentDTO(student);
            return Task.FromResult<ActionResult<StudentDTO>>(Ok(studentDTOs));
        }

        [HttpGet("college")]
        public IActionResult GetStudentByCollege(string? college)
        {
            var students = StudentQueries.GetStudentWithCollege(context.Students.AsQueryable(), college);
            var studentDTOs = MapToStudentsDTO(students);

            if (!studentDTOs.Any())
                return NotFound("No students found for this college");
            return Ok(studentDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> Create(StudentDTO studentDTO)
        {
            var studentId = await _studentService.AddStudent(
                studentDTO.FirstName,
                studentDTO.LastName,
                studentDTO.College,
                studentDTO.Email,
                studentDTO.PhoneNumber
            );
            return CreatedAtAction(nameof(GetStudentById), new { id = studentId }, studentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _studentService.DeleteStudent(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        public async Task<ActionResult> Restore(int id)
        {
            await _studentService.RestoreStudent(id);
            return NoContent();
        }


        [HttpPut("{id}/firstname")]
        public async Task<ActionResult> UpdateFirstName(int id, [FromBody] string fName)
        {
            await _studentService.UpdateStudentFirstName(id, fName);
            return NoContent();
        }

        [HttpPut("{id}/lastname")]
        public async Task<ActionResult> UpdateLastName(int id, [FromBody] string lName)
        {
            await _studentService.UpdateStudentLastName(id, lName);
            return NoContent();
        }

        [HttpPut("{id}/email")]
        public async Task<ActionResult> UpdateEmail(int id, [FromBody] string email)
        {
            await _studentService.UpdateStudentEmail(id, email);
            return NoContent();
        }

        [HttpPut("{id}/college")]
        public async Task<ActionResult> UpdateCollege(int id, [FromBody] string? college)
        {
            await _studentService.UpdateStudentCollege(id, college);
            return NoContent();
        }

        [HttpPut("{id}/phone")]
        public async Task<ActionResult> UpdatePhone(int id, [FromBody] string phone)
        {
            await _studentService.UpdateStudentPhoneNumber(id, phone);
            return NoContent();
        }


        private StudentDTO MapToStudentDTO(Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                College = student.College,
                IsDeleted = student.IsDeleted
            };
        }
        private static List<StudentDTO> MapToStudentsDTO(IEnumerable<Student> students)
        {
            var studentDto = students.Select(c => new StudentDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                College = c.College,
                IsDeleted = c.IsDeleted,
            }).ToList();
            return studentDto;
        }
    }
}
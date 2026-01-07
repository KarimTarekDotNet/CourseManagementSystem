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
            var students = StudentQueries.GetAllStudent(context.Students.AsQueryable());
            var studentDTOs = students.Select(s => new StudentDTO
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                College = s.College,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsDeleted = s.IsDeleted
            }).ToList();
            return Task.FromResult<ActionResult<List<StudentDTO>>>(Ok(studentDTOs));
        }

        [HttpGet("{id}")]
        public Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            var students = StudentQueries.GetAllStudent(context.Students.AsQueryable());
            var student = students.Where(s => s.Id == id).FirstOrDefault();
            if (student == null)
            {
                return Task.FromResult<ActionResult<StudentDTO>>(NotFound());
            }
            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                College = student.College,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                IsDeleted = student.IsDeleted
            };
            return Task.FromResult<ActionResult<StudentDTO>>(Ok(studentDTO));
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

        [HttpPut("{id}/firstname")]
        public async Task<ActionResult> UpdateFirstName(int id, StudentDTO studentDTO)
        {
            await _studentService.UpdateStudentFirstName(id, studentDTO.FirstName);
            return NoContent();
        }

        [HttpPut("{id}/lastname")]
        public async Task<ActionResult> UpdateLastName(int id, StudentDTO studentDTO)
        {
            await _studentService.UpdateStudentLastName(id, studentDTO.LastName);
            return NoContent();
        }

        [HttpPut("{id}/email")]
        public async Task<ActionResult> UpdateEmail(int id, StudentDTO studentDTO)
        {
            await _studentService.UpdateStudentEmail(id, studentDTO.Email);
            return NoContent();
        }

        [HttpPut("{id}/college")]
        public async Task<ActionResult> UpdateCollege(int id, StudentDTO studentDTO)
        {
            await _studentService.UpdateStudentCollege(id, studentDTO.College);
            return NoContent();
        }

        [HttpPut("{id}/phone")]
        public async Task<ActionResult> UpdatePhone(int id, StudentDTO studentDTO)
        {
            await _studentService.UpdateStudentPhoneNumber(id, studentDTO.PhoneNumber);
            return NoContent();
        }
    }
}
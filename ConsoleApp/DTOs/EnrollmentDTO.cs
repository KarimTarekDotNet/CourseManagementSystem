using Project.Entities.Enums;

namespace ConsoleApp.DTOs
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }

        public DateTime StartEnrollmentDate { get; set; }

        public DateTime EndEnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int StudentId { get; set; }

        public string StudentName { get; set; } = null!;
    }
}

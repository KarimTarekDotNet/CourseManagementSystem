using Project.Entities.Enums;

namespace ProjectApi.DTOs
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }

        public DateTime StartEnrollmentDate { get; set; }

        public DateTime EndEnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        public string CourseName { get; set; } = null!;

        public string StudentName { get; set; } = null!;
    }
}

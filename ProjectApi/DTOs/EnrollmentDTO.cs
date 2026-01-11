using Project.Entities.Enums;

namespace ProjectApi.DTOs
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
    public class EnrollmentCreateDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime StartEnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
    public class RestoreEnrollmentDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

}

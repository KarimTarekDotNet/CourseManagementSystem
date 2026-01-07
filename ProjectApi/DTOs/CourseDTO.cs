using Project.Entities.Enums;

namespace ProjectApi.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int TotalHours { get; set; }

        public int SessionDuration { get; set; }

        public int Capacity { get; set; }

        public int? InstructorId { get; set; }

        public CourseLevel Level { get; set; }

        public bool IsDeleted { get; set; }

        public string? InstructorName { get; set; }
    }
}

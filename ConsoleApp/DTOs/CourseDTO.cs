using System.ComponentModel.DataAnnotations;
using Project.Entities.Enums;

namespace ConsoleApp.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Range(1, 1000, ErrorMessage = "Total hours must be between 1 and 1000")]
        public int TotalHours { get; set; }

        [Range(1, 24, ErrorMessage = "Session duration must be between 1 and 24 hours")]
        public int SessionDuration { get; set; }

        [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Course level is required")]
        public CourseLevel Level { get; set; }

        public int? InstructorId { get; set; }

        public string? InstructorName { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Instructor: {InstructorName ?? "UNKNOWN"}";
        }
    }
}
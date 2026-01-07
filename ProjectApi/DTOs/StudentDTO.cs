using System.ComponentModel.DataAnnotations;

namespace ProjectApi.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? College { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ConsoleApp.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "name is required")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "name must be between 3 and 25 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "name is required")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "name must be between 3 and 25 characters")]
        public string LastName { get; set; } = null!;

        [StringLength(20, ErrorMessage = "College cannot exceed 20 characters")]
        public string? College { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 80, ErrorMessage = "email must be 80 characters")]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(maximumLength: 11, ErrorMessage = "phone must be 11 numbers")]
        public string PhoneNumber { get; set; } = null!;

        public bool IsDeleted { get; set; }


        public override string ToString()
        {
            return $"{Id} - {FirstName} {LastName} - {Email} - {PhoneNumber} - {College ?? "UNKNOWN"}";
        }
    }
}

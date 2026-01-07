namespace ProjectApi.DTOs
{
    public class InstructorDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? Department { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}

using Project.Entities;

namespace Project.Interfaces.ServiceInterface
{
    internal interface IInstructorRepository
    {
        Task AddInstructor(string fName, string lName, string? department, string email, string phoneNumber);
        Task RemoveInstructor(int instructorId);
        Task UpdateInstructorFirstName(int instructorId, string fName);
        Task UpdateInstructorLastName(int instructorId, string lName);
        Task UpdateInstructorDepartment(int instructorId, string? department);
        Task UpdateInstructorEmail(int instructorId, string email);
        Task UpdateInstructorPhoneNumber(int instructorId, string phoneNumber);
        Task AssignCourse(int instructorId, int courseId);
        Task RemoveCourse(int instructorId, int courseId);
    }
}

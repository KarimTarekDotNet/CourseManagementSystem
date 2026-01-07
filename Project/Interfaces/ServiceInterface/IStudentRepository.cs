namespace Project.Interfaces.ServiceInterface
{
    internal interface IStudentRepository
    {
        Task<int> AddStudent(string firstName, string lastName, string? college, string email, string phoneNumber);
        Task UpdateStudentFirstName(int id, string firstName);
        Task UpdateStudentLastName(int id, string lastName);
        Task UpdateStudentEmail(int id, string email);
        Task UpdateStudentCollege(int id, string? college);
        Task UpdateStudentPhoneNumber(int id, string phoneNumber);
        Task DeleteStudent(int id);
    }
}

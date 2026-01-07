using Microsoft.EntityFrameworkCore;
using Project.Checks;
using Project.Data;
using Project.Entities;
using Project.Entities.Enums;
using Project.Interfaces.ServiceInterface;

namespace Project.Service
{
    public class StudentService : IStudentRepository
    {
        private readonly AppDbContext Context;
        public StudentService(AppDbContext dbContext)
        {
            Context = dbContext;
        }
        private async Task<Student> GetStudentById(int id)
        {
            Guard.AgainstNonPositive(id);
            var student = await Context.Students.Include(s => s.Enrollments).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
                throw new InvalidOperationException("Student not found");
            return student;
        }
        public async Task<int> AddStudent(string firstName, string lastName, string? college, string email, string phoneNumber)
        {
            var lowerFirstName = firstName.ToLower();
            var lowerLastName = lastName.ToLower();
            var lowerEmail = email.ToLower();

            if (await Context.Students.AnyAsync(s =>
                s.FirstName.ToLower() == lowerFirstName &&
                s.LastName.ToLower() == lowerLastName))
                throw new InvalidOperationException("Student already exists");

            if (await Context.Students.AnyAsync(s => s.Email.ToLower() == lowerEmail))
                throw new InvalidOperationException("Email already in use");

            if (await Context.Students.AnyAsync(s => s.PhoneNumber == phoneNumber))
                throw new InvalidOperationException("Phone number already in use");

            var student = new Student(
                Guard.AgainstNullOrWhiteSpace(firstName),
                Guard.AgainstNullOrWhiteSpace(lastName),
                Guard.AgainstDigitAndMayBeNull(college),
                Guard.AgainstInvalidEmail(email),
                Guard.AgainstInvalidPhone(phoneNumber)
            );

            Context.Students.Add(student);
            await Context.SaveChangesAsync();
            return student.Id;
        }


        public async Task DeleteStudent(int id)
        {
            var student = await GetStudentById(id);

            if (student.Enrollments.Any(x => x.Status == EnrollmentStatus.ACTIVE || x.Status == EnrollmentStatus.COMPLETED))
                throw new InvalidOperationException("Cannot delete student because it has active or completed enrollments.");

            student.SoftDelete();
            await Context.SaveChangesAsync();
        }

        public async Task UpdateStudentCollege(int id, string? college)
        {
            Guard.AgainstDigitAndMayBeNull(college);

            var student = await GetStudentById(id);
            student.UpdateCollege(college);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateStudentEmail(int id, string email)
        {
            Guard.AgainstInvalidEmail(email);
            var student = await GetStudentById(id);
            var lowerEmail = email.ToLower();
            if (await Context.Students.AsAsyncEnumerable().AnyAsync(i => i.Email.ToLower() == lowerEmail && i.Id != id))
                throw new InvalidOperationException("Email already in use by another instructor");
            student.UpdateEmail(email);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateStudentFirstName(int id, string firstName)
        {
            Guard.AgainstNullOrWhiteSpace(firstName);
            var student = await GetStudentById(id);
            student.UpdateFirstName(firstName);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateStudentLastName(int id, string lastName)
        {
            Guard.AgainstNullOrWhiteSpace(lastName);
            var student = await GetStudentById(id);
            student.UpdateLastName(lastName);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateStudentPhoneNumber(int id, string phoneNumber)
        {
            Guard.AgainstInvalidPhone(phoneNumber);
            var student = await GetStudentById(id);
            if (await Context.Students.AnyAsync(s => s.Id != id && s.PhoneNumber == phoneNumber))
                throw new InvalidOperationException("Phone number already in use by another student");
            student.UpdatePhoneNumber(phoneNumber);
            await Context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Project.Checks;
using Project.Data;
using Project.Entities;
using Project.Interfaces.ServiceInterface;
using System;
using System.Threading.Tasks;

namespace Project.Service
{
    public class InstructorService : IInstructorRepository
    {
        private readonly AppDbContext context;
        public InstructorService(AppDbContext dbContext)
        {
            context = dbContext;
        }
        private async Task<Instructor> GetInstructorById(int id)
        {
            Guard.AgainstNonPositive(id);
            var instructor = await context.Instructors.Include(x => x.Courses).FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null)
                throw new InvalidOperationException("Instructor not found");
            return instructor;
        }

        public async Task<int> AddInstructor(string fName, string lName, string? department, string email, string phoneNumber)
        {
            if (await context.Instructors.AnyAsync(s =>
                EF.Functions.Like(s.Email, email)) && await context.Students.AnyAsync(s => EF.Functions.Like(s.Email, email)))
                throw new InvalidOperationException("Email already in use");

            if (await context.Instructors.AnyAsync(s => s.PhoneNumber == phoneNumber) &&
                await context.Students.AnyAsync(s => s.PhoneNumber == phoneNumber))
                throw new InvalidOperationException("Phone number already in use");

            var instructor = new Instructor(
                Guard.AgainstNullOrWhiteSpace(fName),
                Guard.AgainstNullOrWhiteSpace(lName),
                Guard.AgainstDigitAndMayBeNull(department),
                Guard.AgainstInvalidEmail(email),
                Guard.AgainstInvalidPhone(phoneNumber)
            );

            await context.Instructors.AddAsync(instructor);
            await context.SaveChangesAsync();
            return instructor.Id;
        }

        public async Task RemoveInstructor(int instructorId)
        {
            var instructor = await GetInstructorById(instructorId);

            if (instructor.Courses!.Any())
                throw new InvalidOperationException("Cannot delete instructor because they have assigned courses.");

            instructor.SoftDelete();
            await context.SaveChangesAsync();
        }
        public async Task RestoreInstructor(int instructorId)
        {
            Guard.AgainstNonPositive(instructorId);
            var instructor = await context.Instructors.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == instructorId && c.IsDeleted == true);
            if (instructor == null)
                throw new InvalidOperationException("instructor not found");
            instructor.Restore();
            await context.SaveChangesAsync();
        }

        public async Task UpdateInstructorDepartment(int instructorId, string? department)
        {
            Guard.AgainstDigitAndMayBeNull(department);
            var instructor = await GetInstructorById(instructorId);
            instructor.UpdateDepartment(department);
            await context.SaveChangesAsync();
        }

        public async Task UpdateInstructorEmail(int instructorId, string email)
        {
            Guard.AgainstInvalidEmail(email);
            var instructor = await GetInstructorById(instructorId);
            if (await context.Instructors.AnyAsync(i => i.Id != instructorId && EF.Functions.Like(i.Email, email)))
                throw new InvalidOperationException("Email already in use by another instructor");
            instructor.UpdateEmail(email);
            await context.SaveChangesAsync();
        }

        public async Task UpdateInstructorFirstName(int instructorId, string fName)
        {
            Guard.AgainstNullOrWhiteSpace(fName);
            var instructor = await GetInstructorById(instructorId);
            instructor.UpdateFirstName(fName);
            await context.SaveChangesAsync();
        }

        public async Task UpdateInstructorLastName(int instructorId, string lName)
        {
            Guard.AgainstNullOrWhiteSpace(lName);
            var instructor = await GetInstructorById(instructorId);
            instructor.UpdateLastName(lName);
            await context.SaveChangesAsync();
        }

        public async Task UpdateInstructorPhoneNumber(int instructorId, string phoneNumber)
        {
            Guard.AgainstInvalidPhone(phoneNumber);
            var instructor = await GetInstructorById(instructorId);
            if (await context.Instructors.AnyAsync(i => i.Id != instructorId && i.PhoneNumber == phoneNumber))
                throw new InvalidOperationException("Phone number already in use by another instructor");
            instructor.UpdatePhoneNumber(phoneNumber);
            await context.SaveChangesAsync();
        }
        public async Task AssignCourse(int instructorId, int courseId)
        {
            if (!await CanAssignAsync(instructorId, courseId))
                throw new InvalidOperationException("Course already assigned to this instructor");

            var instructor = await GetInstructorById(instructorId);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
                ?? throw new InvalidOperationException("Course not found");

            instructor.AddCourseInstructor(course);
            await context.SaveChangesAsync();
        }
        public async Task RemoveCourse(int instructorId, int courseId)
        {
            if (!await CanRemoveAsync(instructorId, courseId))
                throw new InvalidOperationException("Course not assigned to this instructor");

            var instructor = await GetInstructorById(instructorId);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
                ?? throw new InvalidOperationException("Course not found");

            instructor.RemoveCourseInstructor(course);
            await context.SaveChangesAsync();
        }

        private async Task<bool> CanAssignAsync(int instructorId, int courseId)
        {
            return !await context.Courses.AnyAsync(c =>
                c.Id == courseId &&
                c.InstructorId == instructorId);
        }
        private async Task<bool> CanRemoveAsync(int instructorId, int courseId)
        {
            return await context.Courses.AnyAsync(c =>
                c.Id == courseId &&
                c.InstructorId == instructorId);
        }
    }
}

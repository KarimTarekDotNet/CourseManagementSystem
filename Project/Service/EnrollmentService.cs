using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Entities.Enums;
using Project.Interfaces.ServiceInterface;

namespace Project.Service
{
    public class EnrollmentService : IEnrollmentRepository
    {
        private readonly AppDbContext context;

        public EnrollmentService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Enrollment> CreateEnrollment(
            int studentId,
            int courseId,
            DateTime startDate,
            EnrollmentStatus enrollmentStatus)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            var student = await context.Students
                .FirstOrDefaultAsync(x => x.Id == studentId)
                ?? throw new InvalidOperationException("Student not found");

            var course = await context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == courseId)
                ?? throw new InvalidOperationException("Course not found");

            var existing = await context.Enrollments
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId);

            if (existing != null)
            {
                if (existing.Status != EnrollmentStatus.DROPPED)
                    throw new InvalidOperationException("Student already enrolled");

                existing.UpdateStatus(enrollmentStatus);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existing;
            }

            if (course.Enrollments.Count(e => e.Status != EnrollmentStatus.DROPPED) >= course.Capacity)
                throw new InvalidOperationException("Course is full");

            var enrollment = new Enrollment(student, course, enrollmentStatus, startDate);

            context.Enrollments.Add(enrollment);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return enrollment;
        }

        public async Task<Enrollment> DropEnrollment(int studentId, int courseId)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            var enrollment = await context.Enrollments
                .Include(x => x.Student)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId &&
                    e.Status != EnrollmentStatus.DROPPED)
                ?? throw new InvalidOperationException("Active enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.COMPLETED)
                throw new InvalidOperationException("Enrollment already completed.");

            enrollment.UpdateStatus(EnrollmentStatus.DROPPED);

            try
            {
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return enrollment;
        }
        public async Task RestoreEnrollment(int studentId, int courseId)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            var course = await context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == courseId)
                ?? throw new InvalidOperationException("Course not found");

            var enrollment = await context.Enrollments.IgnoreQueryFilters()
                .Include(x => x.Student)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId);

            if (enrollment == null)
                throw new InvalidOperationException("not found");

            if (enrollment.Status != EnrollmentStatus.DROPPED)
                throw new InvalidOperationException("Enrollment already not dropped.");

            enrollment.Restore(course);

            try
            {
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

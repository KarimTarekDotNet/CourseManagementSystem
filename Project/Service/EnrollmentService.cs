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

        public async Task<Enrollment> CreateEnrollment(int studentId, int courseId, DateTime startDate, EnrollmentStatus enrollmentStatus)
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == studentId)
                ?? throw new InvalidOperationException("Student not found");
            var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == courseId)
                ?? throw new InvalidOperationException("Course not found");

            if (await context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId
            && e.Status != EnrollmentStatus.DROPPED))
            {
                throw new InvalidOperationException(
                    $"Student {student.FullName} is already enrolled in course {course.Name}");
            }

            var enrollment = new Enrollment(student, course, enrollmentStatus, startDate);

            course.AddCourseEnrollment(enrollment);

            await context.SaveChangesAsync();

            return enrollment;
        }
        public async Task<Enrollment> DropEnrollment(int studentId, int courseId)
        {
            var enrollment = await context.Enrollments
                .FirstOrDefaultAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId)
                ?? throw new InvalidOperationException("Enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.DROPPED)
                throw new InvalidOperationException("Enrollment already dropped.");

            if (enrollment.Status == EnrollmentStatus.COMPLETED)
                throw new InvalidOperationException("Enrollment already completed.");

            enrollment.UpdateStatus(EnrollmentStatus.DROPPED);

            await context.SaveChangesAsync();
            return enrollment;
        }
    }
}

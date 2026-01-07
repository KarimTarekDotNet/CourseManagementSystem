using Project.Entities.Enums;

namespace Project.Interfaces.ServiceInterface
{
    internal interface IEnrollmentRepository
    {
        internal Task<Enrollment> CreateEnrollment(int studentId, int courseId, DateTime startDate, EnrollmentStatus enrollmentStatus);
        internal Task<Enrollment> DropEnrollment(int studentId, int courseId);
    }
}

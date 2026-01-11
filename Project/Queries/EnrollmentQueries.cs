using Project.Entities;
using Project.Entities.Enums;

namespace Project.Queries
{
    public static class EnrollmentQueries
    {
        private static void EnsureNotNull(IQueryable<Enrollment> enrollments)
        {
            if (enrollments == null)
                throw new ArgumentNullException(nameof(enrollments));
        }

        public static IQueryable<Enrollment> GetAllEnrollments(
            IQueryable<Enrollment> enrollments)
        {
            EnsureNotNull(enrollments);

            return enrollments;
        }

        public static IQueryable<Enrollment> GetStudentEnrollments(
            IQueryable<Enrollment> enrollments,
            int studentId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.StudentId == studentId);
        }

        public static IQueryable<Enrollment> GetCourseEnrollments(
            IQueryable<Enrollment> enrollments,
            int courseId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.CourseId == courseId);
        }

        public static IQueryable<Enrollment> GetStudentIfAlreadyEnrolled(
            IQueryable<Enrollment> enrollments,
            int studentId,
            int courseId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public static IQueryable<Enrollment> GetEnrollmentsByStatus(
            IQueryable<Enrollment> enrollments,
            EnrollmentStatus enrollmentStatus)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.Status == enrollmentStatus);
        }

        public static IQueryable<Enrollment> GetRecentEnrollmentsForCourse(
            IQueryable<Enrollment> enrollments,
            int courseId,
            int days)
        {
            EnsureNotNull(enrollments);

            var fromDate = DateTime.UtcNow.AddDays(-days);

            return enrollments
                .Where(e => e.CourseId == courseId &&
                            e.StartEnrollmentDate >= fromDate);
        }

        public static IQueryable<Course> GetStudentMostEnrolledCourse(
            IQueryable<Enrollment> enrollments,
            int studentId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.StudentId == studentId)
                .GroupBy(e => e.Course)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(1);
        }

        public static IQueryable<Course> GetDistinctCoursesForStudent(
            IQueryable<Enrollment> enrollments,
            int studentId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.Course)
                .Distinct();
        }

        public static IQueryable<Enrollment> GetEmptyCourseEnrollments(
            IQueryable<Enrollment> enrollments,
            int courseId)
        {
            EnsureNotNull(enrollments);

            return enrollments
                .Where(e => e.CourseId == courseId);
        }
    }
}

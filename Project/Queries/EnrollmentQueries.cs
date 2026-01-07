using Project.Entities;
using Project.Entities.Enums;

namespace Project.Queries
{
    public static class EnrollmentQueries
    {
        private static void EnsureNotEmpty(IQueryable<Enrollment> enrollments)
        {
            if (enrollments == null)
                throw new ArgumentNullException(nameof(enrollments));

            if (!enrollments.Any())
            {
                throw new Exception("The enrollments collection is empty.");
            }
        }

        private static void EnsureNotNull(Student student, Course course)
        {
            if (student == null)
                throw new ArgumentNullException("Student cannot be null.");
            if (course == null)
                throw new ArgumentNullException("Course cannot be null.");
        }

        public static IQueryable<Enrollment> GetAllEnrollment(IQueryable<Enrollment> enrollments)
        {
            EnsureNotEmpty(enrollments);
            return enrollments;
        }

        public static IQueryable<Enrollment> GetStudentEnrollment(IQueryable<Enrollment> enrollments, Student student)
        {
            EnsureNotEmpty(enrollments);

            if (student == null)
                throw new ArgumentNullException("Student cannot be null.");

            return enrollments.Where(x => x.Student == student);
        }

        public static IQueryable<Enrollment> GetCourseEnrollment(IQueryable<Enrollment> enrollments, Course course)
        {
            EnsureNotEmpty(enrollments);

            if (course == null)
                throw new ArgumentNullException("Course cannot be null.");

            return enrollments.Where(x => x.Course == course);
        }

        public static IQueryable<Enrollment> GetStudentIfAlreadyEnrolled(IQueryable<Enrollment> enrollments, Student student, Course course)
        {
            EnsureNotEmpty(enrollments);

            EnsureNotNull(student, course);

            return enrollments
                .Where(x => x.Student == student && x.Course == course);
        }

        public static int CountEnrollmentsForStudentInCourse(IQueryable<Enrollment> enrollments, Student student, Course course)
        {
            EnsureNotEmpty(enrollments);

            if (course == null)
                throw new ArgumentNullException("Course cannot be null.");

            return enrollments.Count(e => e.Student == student && e.Course == course);
        }
        public static int CountCoursesForStudent(IQueryable<Enrollment> enrollments, Student student)
        {
            EnsureNotEmpty(enrollments);

            if (student == null)
                throw new ArgumentNullException("Student cannot be null.");

            return enrollments.Where(e => e.Student == student).Select(e => e.Course).Distinct().Count();
        }

        public static Course? GetStudentMostEnrolledCourse(IQueryable<Enrollment> enrollments, Student student)
        {
            return enrollments
                .Where(x => x.Student == student)
                .GroupBy(x => x.Course)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public static bool EmptyCourse(IQueryable<Enrollment> enrollments, Course course)
        {
            EnsureNotEmpty(enrollments);

            if (course == null)
                throw new ArgumentNullException("Course cannot be null.");

            return !enrollments.Any(x => x.Course == course);
        }
        public static IQueryable<Enrollment> LastEnrollment(IQueryable<Enrollment> enrollments, Course course)
        {
            EnsureNotEmpty(enrollments);

            if (course == null)
                throw new ArgumentNullException("Course cannot be null.");

            var date = DateTime.UtcNow.AddDays(-7);

            return enrollments.Where(x => x.Course == course && x.StartEnrollmentDate >= date);
        }
        public static IQueryable<Enrollment> GetEnrollmentsByStatus(IQueryable<Enrollment> enrollments, EnrollmentStatus enrollmentStatus)
        {
            EnsureNotEmpty(enrollments);
            return enrollments.Where(x => x.Status == enrollmentStatus);
        }
    }
}

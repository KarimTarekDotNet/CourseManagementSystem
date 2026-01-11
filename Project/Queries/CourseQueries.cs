using Microsoft.EntityFrameworkCore;
using Project.Entities;
using Project.Entities.Enums;

namespace Project.Queries
{
    public static class CourseQueries
    {
        private static void EnsureNotNull(IQueryable<Course> courses)
        {
            if (courses == null)
                throw new ArgumentNullException(nameof(courses));
        }

        public static IQueryable<Course> GetAllCourses(
            IQueryable<Course> courses)
        {
            EnsureNotNull(courses);

            return courses;
        }

        public static IQueryable<Course> GetCoursesByTitle(
            IQueryable<Course> courses,
            string title)
        {
            EnsureNotNull(courses);

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            return courses
                .Where(c => EF.Functions.Like(c.Name, $"%{title}%"));
        }

        public static IQueryable<Course> GetCoursesWithLevel(
            IQueryable<Course> courses,
            CourseLevel courseLevel)
        {
            EnsureNotNull(courses);

            return courses
                .Where(c => c.Level == courseLevel);
        }

        public static IQueryable<Course> GetCoursesWithInstructor(
            IQueryable<Course> courses,
            int instructorId)
        {
            EnsureNotNull(courses);

            return courses
                .Where(c => c.Instructor != null && c.Instructor.Id == instructorId);
        }

        public static IQueryable<Course> GetCoursesWithStudents(
            IQueryable<Course> courses,
            int studentId)
        {
            EnsureNotNull(courses);

            return courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .Where(c => c.Enrollments.Any(e => e.Student.Id == studentId));
        }

        public static IQueryable<Course> GetCoursesWithoutInstructor(
            IQueryable<Course> courses)
        {
            EnsureNotNull(courses);

            return courses
                .Where(c => c.Instructor == null);
        }

        public static IQueryable<Course> GetCoursesWithAnyStudents(
            IQueryable<Course> courses)
        {
            EnsureNotNull(courses);

            return courses
                .Where(c => c.Enrollments.Any());
        }

        public static IQueryable<Course> GetCoursesWithMinStudents(
            IQueryable<Course> courses,
            int minStudents)
        {
            EnsureNotNull(courses);

            if (minStudents < 0)
                throw new ArgumentOutOfRangeException(nameof(minStudents));

            return courses
                .Where(c => c.Enrollments.Count() <= minStudents);
        }
    }
}
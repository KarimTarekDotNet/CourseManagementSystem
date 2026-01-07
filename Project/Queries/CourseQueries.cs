    using Microsoft.EntityFrameworkCore;
using Project.Entities;
using Project.Entities.Enums;

namespace Project.Queries
{
    public static class CourseQueries
    {
        private static void EnsureNotEmpty(IQueryable<Course> courses)
        {
            if (courses == null)
                throw new ArgumentNullException(nameof(courses));

            if (!courses.Any())
            {
                throw new Exception("The courses collection is empty.");
            }
        }
        public static IQueryable<Course> GetAllCourses(IQueryable<Course> courses)
        {
            EnsureNotEmpty(courses);
            return courses;
        }

        public static IQueryable<Course> GetCoursesByTitle(IQueryable<Course> courses, string title)
        {
            EnsureNotEmpty(courses);
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            return courses
                .Where(x => EF.Functions.Like(x.Name, title));
        }
        public static IQueryable<Course> GetCoursesWithInstructor(IQueryable<Course> courses, Instructor instructor)
        {
            EnsureNotEmpty(courses);

            if (instructor == null)
                throw new ArgumentNullException("instructor cannot be null.");

            return courses
                .Where(x => x.Instructor == instructor);
        }

        public static IQueryable<Course> GetCoursesWithStudents(IQueryable<Course> courses, Student student)
        {
            EnsureNotEmpty(courses);
            if (student == null)
                throw new ArgumentNullException("Student cannot be null.");
            return courses.Include(x =>x.Enrollments)
                .ThenInclude(x => x.Student)
                .Where(x => x.Enrollments.Any(x => x.Student == student));
        }

        public static IQueryable<Course> GetCoursesWithoutInstructor(IQueryable<Course> courses)
        {
            EnsureNotEmpty(courses);
            return courses.Where(c => c.Instructor == null);
        }

        public static IQueryable<Course> GetCoursesWithAnyStudents(IQueryable<Course> courses)
        {
            EnsureNotEmpty(courses);
            return courses.Where(c => c.Enrollments.Any());
        }

        public static IQueryable<Course> GetCoursesWithMinStudents(
            IQueryable<Course> courses, int minStudents)
        {
            EnsureNotEmpty(courses);
            return courses
                .Where(c => c.Enrollments.OrderBy(x => x.Id).Take(minStudents).Count() <= minStudents);
        }
        public static IQueryable<Course> GetCoursesWithLevel(
            IQueryable<Course> courses, CourseLevel courseLevel)
        {
            EnsureNotEmpty(courses);

            return courses.Where(c => c.Level == courseLevel);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Queries
{
    public static class InstructorQueries
    {
        private static void EnsureNotEmpty(IEnumerable<Instructor> instructors)
        {
            if (instructors == null)
                throw new ArgumentNullException(nameof(instructors));

            if (!instructors.Any())
            {
                throw new Exception("The instructors collection is empty.");
            }
        }

        public static IQueryable<Instructor> GetAllInstructors(
            IQueryable<Instructor> instructors)
        {
            EnsureNotEmpty(instructors);
            return instructors;
        }

        public static IQueryable<Instructor> GetInstructorByName(
            IQueryable<Instructor> instructors,
            string firstName,
            string lastName)
        {
            EnsureNotEmpty(instructors);

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException(
                    "First name cannot be empty.",
                    nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException(
                    "Last name cannot be empty.",
                    nameof(lastName));

            return instructors
                .Where(i =>
                    EF.Functions.Like(i.FirstName, firstName) &&
                    EF.Functions.Like(i.LastName, lastName));
        }

        public static IQueryable<Instructor> GetInstructorByEmail(
            IQueryable<Instructor> instructors,
            string email)
        {
            EnsureNotEmpty(instructors);

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Email cannot be empty.",
                    nameof(email));

            return instructors
                .Where(i => EF.Functions.Like(i.Email, email));
        }

        public static IQueryable<Instructor> GetInstructorsByDepartment(
            IQueryable<Instructor> instructors,
            string department)
        {
            EnsureNotEmpty(instructors);

            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException(
                    "Department cannot be empty.",
                    nameof(department));

            return instructors
                .Where(i => EF.Functions.Like(i.Department ,department));
        }

        public static IQueryable<Instructor> GetInstructorsWithAnyCourses(
            IQueryable<Instructor> instructors)
        {
            EnsureNotEmpty(instructors);

            return instructors
                .Where(i => i.Courses.Any());
        }

        public static IQueryable<Instructor> GetInstructorsWithoutCourses(
            IQueryable<Instructor> instructors)
        {
            EnsureNotEmpty(instructors);

            return instructors
                .Where(i => !i.Courses.Any());
        }
    }
}

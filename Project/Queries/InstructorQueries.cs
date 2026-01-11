using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Queries
{
    public static class InstructorQueries
    {
        private static void EnsureNotNull(IQueryable<Instructor> instructors)
        {
            if (instructors == null)
                throw new ArgumentNullException(nameof(instructors));
        }

        public static IQueryable<Instructor> GetAllInstructors(
            IQueryable<Instructor> instructors)
        {
            EnsureNotNull(instructors);

            return instructors;
        }

        public static IQueryable<Instructor> GetInstructorByName(
            IQueryable<Instructor> instructors,
            string firstName,
            string lastName)
        {
            EnsureNotNull(instructors);

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException(nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException(nameof(lastName));

            return instructors
                .Where(i =>
                    EF.Functions.Like(i.FirstName, $"%{firstName}%") &&
                    EF.Functions.Like(i.LastName, $"%{lastName}%"));
        }

        public static IQueryable<Instructor> GetInstructorByEmail(
            IQueryable<Instructor> instructors,
            string email)
        {
            EnsureNotNull(instructors);

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(nameof(email));

            return instructors
                .Where(i => EF.Functions.Like(i.Email, email));
        }

        public static IQueryable<Instructor> GetInstructorsByDepartment(
            IQueryable<Instructor> instructors,
            string? department)
        {
            EnsureNotNull(instructors);

            if (string.IsNullOrWhiteSpace(department))
            {
                return instructors
                    .Where(i => i.Department == null);
            }

            return instructors
                .Where(i => EF.Functions.Like(i.Department!, $"%{department}%"));
        }

        public static IQueryable<Instructor> GetInstructorsWithAnyCourses(
            IQueryable<Instructor> instructors)
        {
            EnsureNotNull(instructors);

            return instructors
                .Where(i => i.Courses!.Any());
        }

        public static IQueryable<Instructor> GetInstructorsWithoutCourses(
            IQueryable<Instructor> instructors)
        {
            EnsureNotNull(instructors);

            return instructors
                .Where(i => !i.Courses!.Any());
        }
    }
}

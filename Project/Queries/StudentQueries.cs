using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Queries
{
    public static class StudentQueries
    {
        private static void EnsureNotNull(IQueryable<Student> students)
        {
            if (students == null)
                throw new ArgumentNullException(nameof(students));
        }

        public static IQueryable<Student> GetAllStudents(
            IQueryable<Student> students)
        {
            EnsureNotNull(students);

            return students;
        }

        public static Student GetStudentById(
            IQueryable<Student> students, int id)
        {
            EnsureNotNull(students);

            return students.FirstOrDefault(x => x.Id == id) ?? throw new NullReferenceException("not found");
        }

        public static IQueryable<Student> GetStudentWithName(
            IQueryable<Student> students,
            string fName,
            string lName)
        {
            EnsureNotNull(students);

            if (string.IsNullOrWhiteSpace(fName))
                throw new ArgumentNullException(nameof(fName));

            if (string.IsNullOrWhiteSpace(lName))
                throw new ArgumentNullException(nameof(lName));

            return students
                .Where(s =>
                    EF.Functions.Like(s.FirstName, $"%{fName}%") &&
                    EF.Functions.Like(s.LastName, $"%{lName}%"));
        }

        public static Student GetStudentWithEmail(
            IQueryable<Student> students,
            string email)
        {
            EnsureNotNull(students);

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            return students
                .FirstOrDefault(s => EF.Functions.Like(s.Email, email)) ?? throw new NullReferenceException("not found");
        }

        public static Student GetStudentWithPhone(
            IQueryable<Student> students,
            string phone)
        {
            EnsureNotNull(students);

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentNullException(nameof(phone));

            return students
                    .FirstOrDefault(s => s.PhoneNumber == phone) ?? throw new NullReferenceException("not found");
        }

        public static IQueryable<Student> GetStudentWithCollege(
            IQueryable<Student> students,
            string? college)
        {
            EnsureNotNull(students);

            if (string.IsNullOrWhiteSpace(college))
            {
                return students
                    .Where(s => s.College == null);
            }

            return students
                .Where(s => EF.Functions.Like(s.College!, $"%{college}%"));
        }
    }
}

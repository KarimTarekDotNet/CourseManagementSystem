using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Queries
{
    public static class StudentQueries
    {
        private static void EnsureNotEmpty(IQueryable<Student> students)
        {
            if (students == null)
                throw new ArgumentNullException(nameof(students));

            if (!students.Any())
            {
                throw new Exception("The students collection is empty.");
            }
        }

        public static IQueryable<Student> GetAllStudent(IQueryable<Student> students)
        {
            EnsureNotEmpty(students);

            return students;
        }

        public static IQueryable<Student> GetStudentWithName(IQueryable<Student> students, string fName, string lName)
        {
            EnsureNotEmpty(students);

            if (string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName))
                throw new ArgumentNullException("First name or last name cannot be empty.");

            return students.Where(x => EF.Functions.Like(x.FirstName, fName)
            && EF.Functions.Like(x.LastName, lName));
        }

        public static IQueryable<Student> GetStudentWithEmail(IQueryable<Student> students, string email)
        {
            EnsureNotEmpty(students);

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email cannot be empty.");

            return students.Where(x => EF.Functions.Like(x.Email, email));
        }

        public static IQueryable<Student> GetStudentWithPhone(IQueryable<Student> students, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentNullException("Phone cannot be empty.");

            EnsureNotEmpty(students);

            return students.Where(x => x.PhoneNumber == phone);
        }

        public static IQueryable<Student> GetStudentWithCollege(IQueryable<Student> students, string college)
        {
            if (string.IsNullOrWhiteSpace(college))
                throw new ArgumentNullException("college name cannot be empty.");

            EnsureNotEmpty(students);

            return students.Where(x => EF.Functions.Like(x.College, college));
        }
    }
}

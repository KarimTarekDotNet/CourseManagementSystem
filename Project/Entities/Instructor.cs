using Project.Entities.Helper;

namespace Project.Entities
{
    public class Instructor
    {
        private Instructor() { }
        public Instructor(string firstName, string lastName, string? department, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Department = department;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public int Id { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Department { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsDeleted { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public virtual HashSet<Course>? Courses { get; private set; } = new HashSet<Course>();

        override public string ToString()
        {
            return $"Id: {Id} | Name: {FullName} | Department: {Department ?? "UNKNOWN"} | Email: {Email}";
        }

        public void UpdateFirstName(string fName)
        {
            FirstName = EntitiesHelper.UpdateFirstName(fName);
        }

        public void UpdateLastName(string lName)
        {
            LastName = EntitiesHelper.UpdateLastName(lName);
        }

        public void UpdateDepartment(string? department)
        {
            if (department is null)
            {
                Department = null;
                return;
            }

            Department = EntitiesHelper.UpdateStringValue(department, "department");
        }

        public void UpdateEmail(string email)
        {
            Email = EntitiesHelper.UpdateEmail(email);
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = EntitiesHelper.UpdatePhoneNumber(phoneNumber);
        }

        public void AddCourseInstructor(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (Courses != null && Courses.Contains(course))
                throw new InvalidOperationException($"Course {course.Name} is already assigned to this instructor.");

            course.AssignInstructor(this);

            Courses!.Add(course);
        }

        public void RemoveCourseInstructor(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            course.DropInstructor(this);

            Courses!.Remove(course);
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

        public void Restore()
        {
            IsDeleted = false;
        }
    }
}

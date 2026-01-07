using Project.Entities.Helper;

namespace Project.Entities
{
    public class Student
    {
        private Student() { }
        public Student(string firstName, string lastName, string? college, string email, string phoneNumber)
        {
            UpdateFirstName(firstName);
            UpdateLastName(lastName);
            UpdateCollege(college);
            UpdateEmail(email);
            UpdatePhoneNumber(phoneNumber);
        }

        public int Id { get; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string? College { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public string FullName => $"{FirstName} {LastName}";
        public virtual HashSet<Enrollment> Enrollments { get; private set; } = new HashSet<Enrollment>();
        public override string ToString() {return $"Id: {Id} | Name: {FullName} | College: {College ?? "UNKNOWN"}";}

        public void UpdateFirstName(string fName)
        {
            FirstName = EntitiesHelper.UpdateFirstName(fName);
        }

        public void UpdateLastName(string lName)
        {
            LastName = EntitiesHelper.UpdateLastName(lName);
        }

        public void UpdateCollege(string? college)
        {
            if (college is null)
            {
                College = null;
                return;
            }

            College = EntitiesHelper.UpdateStringValue(college, "college");
        }

        public void UpdateEmail(string email)
        {
            Email = EntitiesHelper.UpdateEmail(email);
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = EntitiesHelper.UpdatePhoneNumber(phoneNumber);
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

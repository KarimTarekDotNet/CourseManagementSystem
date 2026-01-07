using Project.Entities.Enums;

namespace Project.Entities
{
    public class Course
    {
        private Course() { }
        public Course(string name, string? description, int totalHours, int sessionDuration, int capacity, CourseLevel courseLevel)
        {
            ChangeName(name);
            ChangeDescription(description);
            ChangeTotalHours(totalHours);
            ChangeSessionDuration(sessionDuration);
            ChangeCapacity(capacity);
            UpdateCourseLevel(courseLevel);
        }
        public int Id { get; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public int TotalHours { get; private set; }
        public int SessionDuration { get; private set; }
        public int Capacity { get; private set; }
        public CourseLevel Level { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public virtual HashSet<Enrollment> Enrollments { get; private set; } = new HashSet<Enrollment>();
        public virtual Instructor? Instructor { get; private set; }
        public int? InstructorId { get; private set; }

        override public string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Level: {Level.ToString()} | Capacity: {Capacity} | Total Hours: {TotalHours} | Instructor: {Instructor?.FullName ?? "UNKNOWN"}";
        }

        public int CalculateNumberOfSessions()
        {
            if (TotalHours <= 0 || SessionDuration <= 0)
            {
                return 0;
            }
            return (int)Math.Ceiling((double)TotalHours / SessionDuration);
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Course name cannot be empty");

            Name = name.Trim();
        }

        public void ChangeDescription(string? description)
        {
            Description = description?.Trim();
        }

        public void ChangeTotalHours(int totalHours)
        {
            if (totalHours <= 0)
                throw new ArgumentException("Total hours must be greater than zero");

            TotalHours = totalHours;
        }

        public void ChangeSessionDuration(int sessionDuration)
        {
            if (sessionDuration <= 0)
                throw new ArgumentException("Session duration must be greater than zero");

            if (TotalHours > 0 && TotalHours % sessionDuration != 0)
                throw new InvalidOperationException(
                    "Session duration must divide total hours exactly");

            SessionDuration = sessionDuration;
        }

        public void ChangeCapacity(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero");

            if (Enrollments.Count > capacity)
                throw new InvalidOperationException(
                    "Capacity cannot be less than current enrollments");

            Capacity = capacity;
        }

        public void UpdateCourseLevel(CourseLevel courseLevel)
        {
            Level = courseLevel;
        }

        public void AddCourseEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            if (Enrollments.Count >= Capacity)
                throw new InvalidOperationException("Course is full");

            if (Enrollments.Any(e => e.StudentId == enrollment.StudentId))
                throw new InvalidOperationException("Student already enrolled");

            Enrollments.Add(enrollment);
        }

        public void RemoveCourseEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

                Enrollments.Remove(enrollment);
        }
        public void AssignInstructor(Instructor instructor)
        {
            if (instructor == null)
                throw new ArgumentNullException(nameof(instructor));

            if (Instructor != null)
                throw new InvalidOperationException("Course already has an instructor.");

            Instructor = instructor;
            InstructorId = instructor.Id;
        }

        public void DropInstructor(Instructor instructor)
        {
            if (instructor == null)
                throw new ArgumentNullException(nameof(instructor));

            if (Instructor?.Id != instructor.Id)
                throw new InvalidOperationException("This instructor is not assigned to this course");

            Instructor = null;
            InstructorId = null;
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

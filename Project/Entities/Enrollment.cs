using Project.Entities;
using Project.Entities.Enums;

public class Enrollment
{
    public int Id { get; }

    public int StudentId { get; private set; }
    public virtual Student Student { get; private set; }

    public int CourseId { get; private set; }
    public virtual Course Course { get; private set; }

    public EnrollmentStatus Status { get; private set; }

    public DateTime StartEnrollmentDate { get; private set; }
    public DateTime EndEnrollmentDate { get; private set; }

    private Enrollment() { }

    public Enrollment(Student student, Course course, EnrollmentStatus status, DateTime startDate)
    {
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Course = course ?? throw new ArgumentNullException(nameof(course));

        StudentId = student.Id;
        CourseId = course.Id;

        Status = status;
        StartEnrollmentDate = startDate;
        EndEnrollmentDate = startDate.AddDays(course.CalculateNumberOfSessions());
    }

    public void UpdateStatus(EnrollmentStatus status)
    {
        if (Status == EnrollmentStatus.COMPLETED)
            throw new InvalidOperationException($"Cannot change enrollment status from {Status} to {status}");

        Status = status;
        StartEnrollmentDate = DateTime.UtcNow;
    }
    public void Restore(Course course)
    {
        if (Status != EnrollmentStatus.DROPPED)
            throw new InvalidOperationException("Only dropped enrollments can be restored");

        Status = EnrollmentStatus.ACTIVE;
        EndEnrollmentDate = StartEnrollmentDate.AddDays(course.CalculateNumberOfSessions());
    }
}
using Microsoft.EntityFrameworkCore;
using Project.Checks;
using Project.Data;
using Project.Entities;
using Project.Entities.Enums;
using Project.Interfaces.ServiceInterface;

namespace Project.Service
{
    public class CourseService : ICourseRepository
    {
        private readonly AppDbContext context;

        public CourseService(AppDbContext dbContext)
        {
           context = dbContext;
        }

        private async Task<Course> GetCourseById(int id)
        {
            Guard.AgainstNonPositive(id);
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                throw new InvalidOperationException("Course not found");
            return course;
        }

        public async Task AddCourse(string name, string? description, int totalHours, int sessionDuration, int capacity, CourseLevel courseLevel)
        {
            var nameLower = name.ToLower();
            if (await context.Courses.AnyAsync(c =>
                EF.Functions.Like(c.Name, nameLower)))
                    throw new InvalidOperationException("Course name already exists");

            var course = new Course(
                Guard.AgainstNullOrWhiteSpace(name),
                Guard.AgainstDigitAndMayBeNull(description),
                Guard.AgainstNonPositive(totalHours),
                Guard.AgainstNonPositive(sessionDuration),
                Guard.AgainstNonPositive(capacity),
                courseLevel
            );
            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCourse(int id)
        {
            Guard.AgainstNonPositive(id);

            var course = await context.Courses
                         .Include(c => c.Enrollments)
                         .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new InvalidOperationException("Course not found");

            if (course.Enrollments.Any(x => x.Status == EnrollmentStatus.ACTIVE || x.Status == EnrollmentStatus.COMPLETED))
                throw new InvalidOperationException("Cannot delete course because it has active or completed enrollments.");

            course.SoftDelete();
            await context.SaveChangesAsync();
        }

        public async Task UpdateCourseCapacity(int id, int capacity)
        {
            Guard.AgainstNonPositive(id);
            Guard.AgainstNonPositive(capacity);
            var course = await GetCourseById(id);
            course.ChangeCapacity(capacity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCourseDescription(int id, string? description)
        {
            Guard.AgainstNonPositive(id);
            var course = await GetCourseById(id);
            Guard.AgainstDigitAndMayBeNull(description);
            course.ChangeDescription(description);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCourseName(int id, string name)
        {
            var nameLower = name.ToLower();
            Guard.AgainstNonPositive(id);
            Guard.AgainstNullOrWhiteSpace(name);
            var course = await GetCourseById(id);
            if (await context.Courses.AnyAsync(c =>
                EF.Functions.Like(c.Name, nameLower) && c.Id != id))
                throw new InvalidOperationException("Course name already exists");
            course.ChangeName(name);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCourseSessionDuration(int id, int sessionDuration)
        {
            Guard.AgainstNonPositive(id);
            Guard.AgainstNonPositive(sessionDuration);
            var course = await GetCourseById(id);
            course.ChangeSessionDuration(sessionDuration);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCourseTotalHours(int id, int totalHours)
        {
            Guard.AgainstNonPositive(id);
            Guard.AgainstNonPositive(totalHours);
            var course = await GetCourseById(id);
            course.ChangeTotalHours(totalHours);
            await context.SaveChangesAsync();
        }
        public async Task UpdateCourseLevel(int id, CourseLevel courseLevel)
        {
            Guard.AgainstNonPositive(id);
            var course = await GetCourseById(id);
            course.UpdateCourseLevel(courseLevel);
            await context.SaveChangesAsync();
        }
    }
}

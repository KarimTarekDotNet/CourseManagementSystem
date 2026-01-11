using Project.Entities.Enums;

namespace Project.Interfaces.ServiceInterface
{
    internal interface ICourseRepository
    {
        Task<int> AddCourse(string name, string? description, int totalHours, int sessionDuration, int capacity, CourseLevel courseLevel);

        Task UpdateCourseName(int id, string name);

        Task UpdateCourseDescription(int id, string? description);

        Task UpdateCourseTotalHours(int id, int totalHours);

        Task UpdateCourseSessionDuration(int id, int sessionDuration);

        Task UpdateCourseCapacity(int id, int capacity);

        Task DeleteCourse(int id);

        Task UpdateCourseLevel(int id, CourseLevel courseLevel);
    }
}

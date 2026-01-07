using Project.Data;
using Project.Entities;
using Project.Entities.Enums;
using Project.Queries;
using Project.Service;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperCoursesMenu
    {
        private readonly CourseService _courseService;

        public HelperCoursesMenu(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task AddCourse()
        {
            try
            {
                Console.WriteLine("=== Add New Course ===");
                Console.Write("Enter course name: ");
                string name = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter course description (optional): ");
                string? description = Console.ReadLine();
                var totalHours = ReadIntInput("Enter total hours: ");
                var sessionDuration = ReadIntInput("Enter session duration (in hours): ");
                int capacity = ReadIntInput("Enter capacity: ");
                int levelInput = ReadIntInput("Enter course level (1- Beginner, 2- Intermediate, 3- Advanced): ");
                if (levelInput < 1 || levelInput > 3)
                    throw new ArgumentOutOfRangeException("Invalid course level.");
                var courseLevel = (CourseLevel)(levelInput - 1);
                await _courseService.AddCourse(name, description, totalHours, sessionDuration, capacity, courseLevel);
                Console.WriteLine("Course added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoveCourse()
        {
            try
            {
                Console.WriteLine("=== Remove Course ===");
                var courseId = ReadIntInput("Enter course ID to remove: ");
                await _courseService.DeleteCourse(courseId);
                Console.WriteLine("Course removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
            }
        }

        public void ViewCourses(AppDbContext context)
        {
            Console.WriteLine("== Show Course ==");
            while (true)
            {
                Console.WriteLine("1. Get All Courses");
                Console.WriteLine("2. Get Courses By Title");
                Console.WriteLine("3. Get Courses Without Instructor");
                Console.WriteLine("4. Get Courses With Min Students");
                Console.WriteLine("5. Get Courses With Level");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("---------------------");

                int choice = ReadIntInput("Select an option: ");
                switch (choice)
                {
                    case 1:
                        var collection = CourseQueries.GetAllCourses(context.Courses.AsQueryable());
                        Console.WriteLine("All Courses");
                        forLoop(collection.ToList());
                        break;
                    case 2:
                        Console.Write("Enter course title to search: ");
                        string title = Console.ReadLine() ?? string.Empty;
                        collection = CourseQueries.GetCoursesByTitle(context.Courses.AsQueryable(), title);
                        forLoop(collection.ToList());
                        break;
                    case 3:
                        Console.WriteLine("Courses Without Instructor:");
                        collection = CourseQueries.GetCoursesWithoutInstructor(context.Courses.AsQueryable());
                        forLoop(collection.ToList());
                        break;
                    case 4:
                        int MinStudent = ReadIntInput("Courses Min Students:");
                        collection = CourseQueries.GetCoursesWithMinStudents(context.Courses.AsQueryable(), MinStudent);
                        forLoop(collection.ToList());
                        break;
                    case 5:
                        try
                        {
                        int level = ReadIntInput("Enter course level (1. Beginner, 2. Intermediate, 3. Advanced): ");
                            if (level < 1 || level > 3)
                                throw new ArgumentOutOfRangeException("Invalid course level.");
                        var courseLevel = (CourseLevel)(level - 1);
                        collection = CourseQueries.GetCoursesWithLevel(context.Courses.AsQueryable(), courseLevel);
                            forLoop(collection.ToList());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public async Task UpdateCourse()
        {
            try
            {
                Console.WriteLine("=== Update Course ===");
                int courseId = ReadIntInput("Enter course ID to update: ");

                while (true)
                {
                    Console.WriteLine("\nSelect property to update:");
                    Console.WriteLine("1. Name");
                    Console.WriteLine("2. Description");
                    Console.WriteLine("3. Total Hours");
                    Console.WriteLine("4. Session Duration");
                    Console.WriteLine("5. Capacity");
                    Console.WriteLine("6. Course Level");
                    Console.WriteLine("0. Exit");
                    Console.WriteLine("---------------------");
                    int choice = ReadIntInput("Your choice: ");

                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter new course name: ");
                            string name = Console.ReadLine() ?? string.Empty;
                            await _courseService.UpdateCourseName(courseId, name);
                            Console.WriteLine("Course name updated successfully.");
                            break;

                        case 2:
                            Console.Write("Enter new course description (optional): ");
                            string? description = Console.ReadLine();
                            await _courseService.UpdateCourseDescription(courseId, description);
                            Console.WriteLine("Course description updated successfully.");
                            break;

                        case 3:
                            int totalHours = ReadIntInput("Enter new total hours: ");
                            await _courseService.UpdateCourseTotalHours(courseId, totalHours);
                            Console.WriteLine("Course total hours updated successfully.");
                            break;

                        case 4:
                            int sessionDuration = ReadIntInput("Enter new session duration: ");
                            await _courseService.UpdateCourseSessionDuration(courseId, sessionDuration);
                            Console.WriteLine("Course session duration updated successfully.");
                            break;

                        case 5:
                            int capacity = ReadIntInput("Enter new capacity: ");
                            await _courseService.UpdateCourseCapacity(courseId, capacity);
                            Console.WriteLine("Course capacity updated successfully.");
                            break;

                        case 6:
                            int levelInput = ReadIntInput("Enter new course level (1- Beginner, 2- Intermediate, 3- Advanced): ");
                            if (levelInput < 1 || levelInput > 3)
                                throw new ArgumentOutOfRangeException("Invalid course level.");
                            var courseLevel = (CourseLevel)levelInput - 1;
                            await _courseService.UpdateCourseLevel(courseId, courseLevel);
                            Console.WriteLine("Course level updated successfully.");
                            break;

                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }



        private int ReadIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        private void forLoop(List<Course> collection)
        {
            if (!collection.Any())
            {
                Console.WriteLine("No Courses found.");
                return;
            }
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}

using ConsoleApp.ApiServices;
using ConsoleApp.DTOs;
using Project.Entities.Enums;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperCoursesMenu
    {
        private readonly CourseApiService _courseApiService;

        public HelperCoursesMenu(CourseApiService courseApiService)
        {
            _courseApiService = courseApiService;
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
                var courseDTO = new CourseDTO
                {
                    Name = name,
                    Description = description,
                    TotalHours = totalHours,
                    SessionDuration = sessionDuration,
                    Capacity = capacity,
                    Level = courseLevel
                };
                await _courseApiService.AddCourse(courseDTO);
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
                await _courseApiService.DeleteCourse(courseId);
                Console.WriteLine("Course removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
            }
        }
        public async Task RestoreCourse()
        {
            try
            {
                Console.WriteLine("=== Restore Course ===");
                var courseId = ReadIntInput("Enter course ID to restore: ");
                await _courseApiService.RestoreCourse(courseId);
                Console.WriteLine("Course restored successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
            }
        }

        public async Task ViewCourses()
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
                        var collection = await _courseApiService.GetAllCourses();
                        Console.WriteLine("All Courses");
                        DisplayStudents(collection);
                        break;
                    case 2:
                        Console.Write("Enter course title to search: ");
                        string title = Console.ReadLine() ?? string.Empty;
                        collection = await _courseApiService.GetCourseByName(title);
                        DisplayStudents(collection);
                        break;
                    case 3:
                        Console.WriteLine("Courses Without Instructor:");
                        collection = await _courseApiService.GetCoursesWithoutInstructor();
                        DisplayStudents(collection);
                        break;
                    case 4:
                        int MinStudent = ReadIntInput("Courses Min Students:");
                        collection = await _courseApiService.GetCoursesWithMinStudents(MinStudent);
                        DisplayStudents(collection);
                        break;
                    case 5:
                        try
                        {
                        int level = ReadIntInput("Enter course level (1. Beginner, 2. Intermediate, 3. Advanced): ");
                            if (level < 1 || level > 3)
                                throw new ArgumentOutOfRangeException("Invalid course level.");
                        var courseLevel = (CourseLevel)(level - 1);
                        collection = await _courseApiService.GetCoursesWithLevel(courseLevel);
                            DisplayStudents(collection);
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
                            await _courseApiService.UpdateName(courseId, name);
                            Console.WriteLine("Course name updated successfully.");
                            break;

                        case 2:
                            Console.Write("Enter new course description (optional): ");
                            string? description = Console.ReadLine();
                            await _courseApiService.UpdateDescription(courseId, description);
                            Console.WriteLine("Course description updated successfully.");
                            break;

                        case 3:
                            int totalHours = ReadIntInput("Enter new total hours: ");
                            await _courseApiService.UpdateTotalHours(courseId, totalHours);
                            Console.WriteLine("Course total hours updated successfully.");
                            break;

                        case 4:
                            int sessionDuration = ReadIntInput("Enter new session duration: ");
                            await _courseApiService.UpdateSessionDuration(courseId, sessionDuration);
                            Console.WriteLine("Course session duration updated successfully.");
                            break;

                        case 5:
                            int capacity = ReadIntInput("Enter new capacity: ");
                            await _courseApiService.UpdateCapacity(courseId, capacity);
                            Console.WriteLine("Course capacity updated successfully.");
                            break;

                        case 6:
                            int levelInput = ReadIntInput("Enter new course level (1- Beginner, 2- Intermediate, 3- Advanced): ");
                            if (levelInput < 1 || levelInput > 3)
                                throw new ArgumentOutOfRangeException("Invalid course level.");
                            var courseLevel = (CourseLevel)levelInput - 1;
                            await _courseApiService.UpdateLevel(courseId, courseLevel);
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
        private void DisplayStudents(List<CourseDTO> courseDTOs, int pageSize = 10)
        {
            if (courseDTOs == null || courseDTOs.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(courseDTOs.Count / (double)pageSize);
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1}/{totalPages}");
                Console.WriteLine("-------------------------");
                var pageItems = courseDTOs.Skip(currentPage * pageSize).Take(pageSize).ToList();

                foreach (var s in pageItems)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("\nUse Left/Right arrows to navigate, Esc to exit.");

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.RightArrow && currentPage < totalPages - 1)
                    currentPage++;

                else if (key == ConsoleKey.LeftArrow && currentPage > 0)
                    currentPage--;

                else
                    break;
            }
        }
    }
}

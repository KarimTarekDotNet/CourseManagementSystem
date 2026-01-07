using ConsoleApp.ConsoleHelper.HelperMenus;
using Project.Data;
using Project.Service;

namespace ConsoleApp.ConsoleHelper
{
    public static class CoursesMenu
    {
        public static async Task DisplayCoursesMenu(AppDbContext context)
        {
            var courseService = new CourseService(context);
            var helperCoursesMenu = new HelperCoursesMenu(courseService);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Courses Menu ===");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. View Courses");
                Console.WriteLine("3. Update Course");
                Console.WriteLine("4. Delete Course");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");
                int choice;
                while(int.TryParse(Console.ReadLine(), out choice) == false)
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");
                    Console.Write("Select an option: ");
                }
                switch (choice)
                {
                    case 1:
                        await helperCoursesMenu.AddCourse();
                        break;
                    case 2:
                        helperCoursesMenu.ViewCourses(context);
                        break;
                    case 3:
                        await helperCoursesMenu.UpdateCourse();
                        break;
                    case 4:
                        await helperCoursesMenu.RemoveCourse();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}

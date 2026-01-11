using ConsoleApp.ApiServices;

namespace ConsoleApp.ConsoleHelper
{
    public static class Login
    {
        public static async Task MenuAsync(InstructorApiService instructorApiService, EnrollmentApiService enrollmentApiService
            , StudentApiService studentApiService, CourseApiService courseApiService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Course Academy Management ===");
                Console.WriteLine("1. Manage courses");
                Console.WriteLine("2. Manage Students");
                Console.WriteLine("3. Manage Enrollment");
                Console.WriteLine("4. Manage Instrutor");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
                    Console.Write("Select an option: ");
                }
                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;
                    case 1:
                        await CoursesMenu.DisplayCoursesMenu(courseApiService);
                        break;
                    case 2:
                        await StudentsMenu.DisplayStudentsMenu(studentApiService);
                        break;
                    case 3:
                        await EnrollmentMenu.DisplayEnrollmentMenu(enrollmentApiService);
                        break;
                    case 4:
                        await InstructorMenu.DisplayInstructorMenu(instructorApiService);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }
    }
}
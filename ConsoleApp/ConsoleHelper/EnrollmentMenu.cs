using ConsoleApp.ConsoleHelper.HelperMenus;
using Project.Data;
using Project.Service;

namespace ConsoleApp.ConsoleHelper
{
    public static class EnrollmentMenu
    {
        public static async Task DisplayEnrollmentMenu(AppDbContext context)
        {
            var enrollmentService = new EnrollmentService(context);
            var helperCoursesMenu = new EnrollHelperMenu(enrollmentService);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enrollment Menu:");
                Console.WriteLine("1. Enroll Student in Course");
                Console.WriteLine("2. Drop Student from Course");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Please select an option: ");
                int choice;
                while (int.TryParse(Console.ReadLine(), out choice) == false)
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");
                    Console.Write("Select an option: ");
                }
                switch (choice)
                {
                    case 1:
                        await helperCoursesMenu.EnrollStudentInCourse();
                        break;
                    case 2:
                        await helperCoursesMenu.DropStudentFromCourse();
                        break;
                    case 3:
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
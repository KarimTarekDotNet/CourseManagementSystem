using ConsoleApp.ConsoleHelper.HelperMenus;
using ConsoleApp.ApiServices;

namespace ConsoleApp.ConsoleHelper
{
    public static class EnrollmentMenu
    {
        public static async Task DisplayEnrollmentMenu(EnrollmentApiService enrollmentApiService)
        {
            var helperEnrollmentMenu = new HelperEnrollmentMenu(enrollmentApiService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enrollment Menu:");
                Console.WriteLine("1. Enroll Student in Course");
                Console.WriteLine("2. Drop Student from Course");
                Console.WriteLine("3. Restore Student from Course");
                Console.WriteLine("4. View Enrollments by Student");
                Console.WriteLine("5. View Enrollments by Course");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Please select an option: ");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");
                    Console.Write("Select an option: ");
                }

                switch (choice)
                {
                    case 1:
                        await helperEnrollmentMenu.EnrollStudentInCourse();
                        break;

                    case 2:
                        await helperEnrollmentMenu.DropStudentFromCourse();
                        break;

                    case 3:
                        await helperEnrollmentMenu.RestoreStudentFromCourse();
                        break;

                    case 4:
                        await helperEnrollmentMenu.ViewByStudent();
                        break;

                    case 5:
                        await helperEnrollmentMenu.ViewByCourse();
                        break;

                    case 0:
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

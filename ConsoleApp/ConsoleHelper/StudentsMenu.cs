using ConsoleApp.ApiServices;
using ConsoleApp.ConsoleHelper.HelperMenus;
using Project.Data;
using Project.Service;

namespace ConsoleApp.ConsoleHelper
{
    public static class StudentsMenu
    {
        internal static async Task DisplayStudentsMenu(StudentApiService studentApiService)
        {
            var helperStudentMenu = new HelperStudentMenu(studentApiService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Students Menu ===");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Restore Student");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                int choice;
                while (int.TryParse(Console.ReadLine(), out choice) == false)
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");
                    Console.Write("Select an option: ");
                }

                switch (choice)
                {
                    case 1:
                        await helperStudentMenu.AddStudent();
                        break;

                    case 2:
                        await helperStudentMenu.ViewStudents();
                        break;

                    case 3:
                        await helperStudentMenu.UpdateStudent();
                        break;

                    case 4:
                        await helperStudentMenu.RemoveStudent();
                        break;

                    case 5:
                        await helperStudentMenu.RestoreStudent();
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

using ConsoleApp.ConsoleHelper.HelperMenus;
using Project.Data;
using Project.Service;

namespace ConsoleApp.ConsoleHelper
{
    public static class InstructorMenu
    {
        public static async Task DisplayInstructorMenu(AppDbContext context)
        {
            var instructorService = new InstructorService(context);
            var helperInstructorMenu = new HelperInstructorMenu(instructorService);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Instructors Menu ===");
                Console.WriteLine("1. Add Instructor");
                Console.WriteLine("2. View Instructors");
                Console.WriteLine("3. Update Instructor");
                Console.WriteLine("4. Delete Instructor");
                Console.WriteLine("5. Assign Course");
                Console.WriteLine("6. Remove Course");
                Console.WriteLine("7. Back to Main Menu");
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
                        await helperInstructorMenu.AddInstructor();
                        break;
                    case 2:
                        helperInstructorMenu.ViewInstructors(context);
                        break;
                    case 3:
                        await helperInstructorMenu.UpdateInstructor();
                        break;
                    case 4:
                        await helperInstructorMenu.RemoveInstructor();
                        break;
                    case 5:
                        await helperInstructorMenu.AssignCourse();
                        break;
                    case 6:
                        await helperInstructorMenu.RemoveCourse();
                        break;
                    case 7:
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
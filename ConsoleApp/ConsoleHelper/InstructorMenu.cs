using ConsoleApp.ApiServices;
using ConsoleApp.ConsoleHelper.HelperMenus;

namespace ConsoleApp.ConsoleHelper
{
    public static class InstructorMenu
    {
        public static async Task DisplayInstructorMenu(InstructorApiService instructorApiService)
        {
            var helperInstructorMenu = new HelperInstructorMenu(instructorApiService);

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
                Console.WriteLine("7. Restore Instructor");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        await helperInstructorMenu.AddInstructor();
                        break;

                    case 2:
                        await helperInstructorMenu.ViewInstructors();
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
                        await helperInstructorMenu.RestoreInstructor();
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}

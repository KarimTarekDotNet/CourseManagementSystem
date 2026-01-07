using ConsoleApp.ConsoleHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Data;

namespace ConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var oprions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            using var context = new AppDbContext(oprions);
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
                while(!int.TryParse(Console.ReadLine(), out choice))
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
                        await CoursesMenu.DisplayCoursesMenu(context);
                        break;
                    case 2:
                        await StudentsMenu.DisplayStudentsMenu(context);
                        break;
                    case 3:
                        await EnrollmentMenu.DisplayEnrollmentMenu(context);
                        break;
                    case 4:
                        await InstructorMenu.DisplayInstructorMenu(context);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }
    }
}
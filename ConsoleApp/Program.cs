using ConsoleApp.ApiServices;
using ConsoleApp.ConsoleHelper;
using ConsoleApp.ConsoleHelper.HelperMenus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Data;
using ProjectApi.DTOs;

namespace ConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.App.json").Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var oprions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            var login = new LoginApiService();

            Console.WriteLine("=== Course Academy Management ===");
            int tryLogin = 5;
            while (tryLogin-- > 0)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Email: ");
                string email = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Password: ");
                string password = PassHelper.ReadPassword();

                var admin = new AdminDTO
                {
                    Username = username,
                    Email = email,
                    Password = password
                };
                if (await login.Login(admin))
                {
                    Console.WriteLine("Welcome!");
                    var courseApi = new CourseApiService(login.Token!);
                    var enrollApi = new EnrollmentApiService(login.Token!);
                    var studentApi = new StudentApiService(login.Token!);
                    var instructorApi = new InstructorApiService(login.Token!);
                    await Login.MenuAsync(instructorApi, enrollApi, studentApi, courseApi);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("try Again!");  
                }
            }
        }
    }
}
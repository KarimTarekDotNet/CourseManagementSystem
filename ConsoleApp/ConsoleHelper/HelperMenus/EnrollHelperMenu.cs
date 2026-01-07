using Project.Entities;
using Project.Entities.Enums;
using Project.Service;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class EnrollHelperMenu
    {
        private readonly EnrollmentService _enrollmentService;
        public EnrollHelperMenu(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        public async Task EnrollStudentInCourse()
        {
            try
            {
                Console.WriteLine("=== Enroll Student In Course ===");
                int studentId = ReadDateInput("Enter Student Id: ");
                int courseId = ReadDateInput("Enter Course Id: ");
                var startDate = DateTime.UtcNow;
                int status = ReadDateInput("Enter Enrollment Status (1: Active, 2: Dropped, 3: Completed): ");
                await _enrollmentService.CreateEnrollment(studentId, courseId, startDate, (EnrollmentStatus)(status - 1));
                Console.WriteLine($"Enrollment added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task DropStudentFromCourse()
        {
            try
            {
                Console.WriteLine("=== Drop Student From Course ===");
                int studentId = ReadDateInput("Enter Student Id: ");
                int courseId = ReadDateInput("Enter Course Id: ");
                await _enrollmentService.DropEnrollment(studentId, courseId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private int ReadDateInput(string prompt)
        {
            int value;
            Console.Write(prompt);
            while (int.TryParse(Console.ReadLine(), out value) == false)
            {
                Console.WriteLine("Invalid input. Please enter a valid Course Id.");
                Console.Write("Enter Course Id: ");
            }
            return value;
        }
    }
}

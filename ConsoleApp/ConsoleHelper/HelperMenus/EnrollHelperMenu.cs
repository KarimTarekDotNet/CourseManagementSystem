using ConsoleApp.ApiServices;
using ConsoleApp.DTOs;
using Project.Entities.Enums;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperEnrollmentMenu
    {
        private readonly EnrollmentApiService _enrollmentApi;

        public HelperEnrollmentMenu(EnrollmentApiService enrollmentApi)
        {
            _enrollmentApi = enrollmentApi;
        }

        public async Task EnrollStudentInCourse()
        {
            try
            {
                Console.WriteLine("=== Enroll Student In Course ===");

                int studentId = ReadIntInput("Student ID: ");
                int courseId = ReadIntInput("Course ID: ");

                Console.WriteLine("Status (1: Active, 2: Dropped, 3: Completed)");
                int statusInput = ReadIntInput("Choose: ");

                await _enrollmentApi.Create(studentId, courseId, DateTime.UtcNow, (EnrollmentStatus)(statusInput - 1));
                Console.WriteLine("Enrollment added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DropStudentFromCourse()
        {
            try
            {
                Console.WriteLine("=== Drop Student From Course ===");

                int studentId = ReadIntInput("Student ID: ");
                int courseId = ReadIntInput("Course ID: ");

                await _enrollmentApi.Drop(studentId, courseId);
                Console.WriteLine("Enrollment dropped successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RestoreStudentFromCourse()
        {
            try
            {
                Console.WriteLine("=== Restore Student From Course ===");

                int studentId = ReadIntInput("Student ID: ");
                int courseId = ReadIntInput("Course ID: ");

                await _enrollmentApi.Restore(studentId, courseId);
                Console.WriteLine("Enrollment Restore successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        public async Task ViewByStudent()
        {
            int studentId = ReadIntInput("Student ID: ");
            var enrollments = await _enrollmentApi.GetStudentEnrollments(studentId);
            PrintEnrollments(enrollments);
        }

        public async Task ViewByCourse()
        {
            int courseId = ReadIntInput("Course ID: ");
            var enrollments = await _enrollmentApi.GetCourseEnrollments(courseId);
            PrintEnrollments(enrollments);
        }

        /* ========= Helpers ========= */

        private int ReadIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Invalid number, try again.");
            }
        }

        private void PrintEnrollments(List<EnrollmentDTO> enrollDTOs, int pageSize = 10)
        {
            if (enrollDTOs == null || enrollDTOs.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(enrollDTOs.Count / (double)pageSize);
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1}/{totalPages}");
                Console.WriteLine("-------------------------");
                var pageItems = enrollDTOs.Skip(currentPage * pageSize).Take(pageSize).ToList();

                foreach (var s in pageItems)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("\nUse Left/Right arrows to navigate, Esc to exit.");

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.RightArrow && currentPage < totalPages - 1)
                    currentPage++;

                else if (key == ConsoleKey.LeftArrow && currentPage > 0)
                    currentPage--;

                else
                    break;
            }
        }
    }
}

using ConsoleApp.ApiServices;
using ConsoleApp.DTOs;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperInstructorMenu
    {
        private readonly InstructorApiService _instructorApi;

        public HelperInstructorMenu(InstructorApiService instructorApi)
        {
            _instructorApi = instructorApi;
        }

        public async Task AddInstructor()
        {
            try
            {
                Console.WriteLine("=== Add New Instructor ===");

                Console.Write("First Name: ");
                string firstName = Console.ReadLine() ?? string.Empty;

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine() ?? string.Empty;

                Console.Write("Department (optional): ");
                string? department = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine() ?? string.Empty;

                Console.Write("Phone: ");
                string phone = Console.ReadLine() ?? string.Empty;

                var dto = new InstructorDTO
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Department = string.IsNullOrWhiteSpace(department) ? null : department,
                    Email = email,
                    PhoneNumber = phone
                };

                await _instructorApi.Create(dto);

                Console.WriteLine("Instructor added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoveInstructor()
        {
            try
            {
                int id = ReadIntInput("Enter Instructor ID: ");
                await _instructorApi.Delete(id);
                Console.WriteLine("Instructor deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RestoreInstructor()
        {
            try
            {
                int id = ReadIntInput("Enter Instructor ID: ");
                await _instructorApi.Restore(id);
                Console.WriteLine("Instructor restored successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateInstructor()
        {
            int id = ReadIntInput("Enter Instructor ID: ");
            while (true)
            {
                Console.WriteLine("=== Update Instructor ===");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Phone");
                Console.WriteLine("5. Department");
                Console.WriteLine("0. Back");

                int choice = ReadIntInput("Select option: ");
                if (choice == 0) return;


                try
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("New First Name: ");
                            await _instructorApi.UpdateFirstName(id, Console.ReadLine() ?? "");
                            break;

                        case 2:
                            Console.Write("New Last Name: ");
                            await _instructorApi.UpdateLastName(id, Console.ReadLine() ?? "");
                            break;

                        case 3:
                            Console.Write("New Email: ");
                            await _instructorApi.UpdateLastEmail(id, Console.ReadLine() ?? "");
                            break;

                        case 4:
                            Console.Write("New Phone: ");
                            await _instructorApi.UpdatePhone(id, Console.ReadLine() ?? "");
                            break;

                        case 5:
                            Console.Write("New Department: ");
                            string? dept = Console.ReadLine();
                            await _instructorApi.UpdateDepartment(id, dept);
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }

                    Console.WriteLine("Instructor updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        public async Task ViewInstructors()
        {
            while (true)
            {
                Console.WriteLine("=== View Instructors ===");
                Console.WriteLine("1. View by Department");
                Console.WriteLine("2. With Courses");
                Console.WriteLine("3. Without Courses");
                Console.WriteLine("0. Back");

                int choice = ReadIntInput("Select option: ");
                if (choice == 0) return;

                try
                {
                    switch (choice)
                    {
                        case 1:
                            await ViewByDepartment();
                            break;

                        case 2:
                            await GetWithCourses();
                            break;

                        case 3:
                            await GetWithoutCourses();
                            break;

                        case 0:
                            return;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task ViewByDepartment()
        {
            Console.Write("Department (leave empty for all): ");
            string? department = Console.ReadLine();
            var instructors = await _instructorApi.GetByDepartment(department);
            PrintInstructors(instructors);
        }
        private async Task GetWithCourses()
        {
            var Instructors = await _instructorApi.GetWithCourses();
            PrintInstructors(Instructors);
        }
        private async Task GetWithoutCourses()
        {
            var Instructors = await _instructorApi.GetWithoutCourses();
            PrintInstructors(Instructors);
        }


        public async Task AssignCourse()
        {
            try
            {
                int instructorId = ReadIntInput("Instructor ID: ");
                int courseId = ReadIntInput("Course ID: ");

                await _instructorApi.AssignCourse(instructorId, courseId);
                Console.WriteLine("Course assigned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoveCourse()
        {
            try
            {
                int instructorId = ReadIntInput("Instructor ID: ");
                int courseId = ReadIntInput("Course ID: ");

                await _instructorApi.DropCourse(instructorId, courseId);
                Console.WriteLine("Course removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

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

        private void PrintInstructors(List<InstructorDTO> instructorDTOs, int pageSize = 10)
        {
            if (instructorDTOs == null || instructorDTOs.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(instructorDTOs.Count / (double)pageSize);
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1}/{totalPages}");
                Console.WriteLine("-------------------------");
                var pageItems = instructorDTOs.Skip(currentPage * pageSize).Take(pageSize).ToList();

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

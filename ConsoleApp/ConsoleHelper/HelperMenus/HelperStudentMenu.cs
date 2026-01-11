using ConsoleApp.ApiServices;
using ConsoleApp.DTOs;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperStudentMenu
    {
        private readonly StudentApiService _studentApiService;

        public HelperStudentMenu(StudentApiService studentApiService)
        {
            _studentApiService = studentApiService;
        }

        public async Task AddStudent()
        {
            try
            {
                Console.WriteLine("=== Add New Student ===");
                Console.Write("Enter Student First name: ");
                string fName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Student Last name: ");
                string lName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Student College (optional): ");
                string? college = Console.ReadLine();
                Console.Write("Enter Student Email: ");
                string email = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Student Phone: ");
                string phone = Console.ReadLine() ?? string.Empty;
                var studentDTO = new StudentDTO
                {
                    FirstName = fName,
                    LastName = lName,
                    College = college,
                    Email = email,
                    PhoneNumber = phone
                };
                await _studentApiService.AddStudent(studentDTO);

                Console.WriteLine($"{fName + " " + lName} added successfully.");
                Console.WriteLine($"if you want your Id please search in view students -> view by name");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task RemoveStudent()
        {
            try
            {
                Console.WriteLine("=== Delete Student ===");
                int studentId = ReadIntInput("Enter Student Id: ");
                await _studentApiService.DeleteStudent(studentId);
                Console.WriteLine("remove successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task RestoreStudent()
        {
            try
            {
                Console.WriteLine("=== Restore Student ===");
                int studentId = ReadIntInput("Enter Student Id: ");
                await _studentApiService.RestoreStudent(studentId);
                Console.WriteLine("Restore successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task UpdateStudent()
        {
            try
            {
                int studentId = ReadIntInput("Enter Student Id: ");
                while (true)
                {
                    Console.WriteLine("=== Update Student ===");
                    Console.WriteLine("1. update first name");
                    Console.WriteLine("2. update last name");
                    Console.WriteLine("3. update email");
                    Console.WriteLine("4. update phone");
                    Console.WriteLine("5. update college");
                    Console.WriteLine("0. return menu");
                    int choice = ReadIntInput("Select an option: ");
                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                Console.Write("Enter new First name: ");
                                string fName = Console.ReadLine() ?? string.Empty;
                                await _studentApiService.UpdateStudentFirstName(studentId, fName);
                                Console.WriteLine("Update Successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            break;
                        case 2:
                            try
                            {
                                Console.Write("Enter new last name: ");
                                string lName = Console.ReadLine() ?? string.Empty;
                                await _studentApiService.UpdateStudentLastName(studentId, lName);
                                Console.WriteLine("Update Successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 3:
                            try
                            {
                                Console.Write("Enter new email: ");
                                string email = Console.ReadLine() ?? string.Empty;
                                await _studentApiService.UpdateStudentEmail(studentId, email);
                                Console.WriteLine("Update Successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 4:
                            try
                            {
                                Console.Write("Enter new phone number: ");
                                string phone = Console.ReadLine() ?? string.Empty;
                                await _studentApiService.UpdateStudentPhoneNumber(studentId, phone);
                                Console.WriteLine("Update Successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 5:
                            try
                            {
                                Console.Write("Enter new college: ");
                                string college = Console.ReadLine() ?? string.Empty;
                                await _studentApiService.UpdateStudentCollege(studentId, college);
                                Console.WriteLine("Update Successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 0:
                            return;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task ViewStudents()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("=== View Students ===");
                    Console.WriteLine("1. View all students");
                    Console.WriteLine("2. View by name");
                    Console.WriteLine("3. View by email");
                    Console.WriteLine("4. View by phone");
                    Console.WriteLine("5. View by college");
                    Console.WriteLine("0. Return menu");

                    int choice = ReadIntInput("Select an option: ");

                    switch (choice)
                    {
                        case 1:
                            var students = await _studentApiService.GetAllStudents();
                            DisplayStudents(students);
                            break;

                        case 2:
                            Console.Write("Enter First name: ");
                            string fName = Console.ReadLine() ?? string.Empty;
                            Console.Write("Enter Last name: ");
                            string lName = Console.ReadLine() ?? string.Empty;

                            students = await _studentApiService.GetStudentsByName(fName, lName);
                            DisplayStudents(students);
                            break;

                        case 3:
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine() ?? string.Empty;

                            var student = await _studentApiService.GetStudentByEmail(email);
                            Console.WriteLine(student);
                            break;

                        case 4:
                            Console.Write("Enter Phone: ");
                            string phone = Console.ReadLine() ?? string.Empty;

                            student = await _studentApiService.GetStudentByPhone(phone);
                            Console.WriteLine(student);
                            break;

                        case 5:
                            Console.Write("Enter College: ");
                            string college = Console.ReadLine() ?? string.Empty;

                            students = await _studentApiService.GetStudentsByCollege(college);
                            DisplayStudents(students);
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private int ReadIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        private void DisplayStudents(List<StudentDTO> studentDTOs, int pageSize = 10)
        {
            if (studentDTOs == null || studentDTOs.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(studentDTOs.Count / (double)pageSize);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1}/{totalPages} - Total Students: {studentDTOs.Count}");
                Console.WriteLine("=".PadRight(50, '='));

                var pageItems = studentDTOs.Skip(currentPage * pageSize).Take(pageSize).ToList();

                foreach (var s in pageItems)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("=".PadRight(50, '='));
                Console.WriteLine("Use Left/Right arrows to navigate, Esc to exit.");

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                    break;
                else if (key == ConsoleKey.RightArrow && currentPage < totalPages - 1)
                    currentPage++;
                else if (key == ConsoleKey.LeftArrow && currentPage > 0)
                    currentPage--;
            }
        }
    }
}

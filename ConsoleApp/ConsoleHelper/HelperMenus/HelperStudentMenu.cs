using Project.Data;
using Project.Entities;
using Project.Queries;
using Project.Service;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperStudentMenu
    {
        private readonly StudentService _studentService;

        public HelperStudentMenu(StudentService studentService)
        {
            _studentService = studentService;
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
                await _studentService.AddStudent(fName, lName, college, email, phone);
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
                await _studentService.DeleteStudent(studentId);
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
                    int studentId = ReadIntInput("Enter Student Id: ");
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter new First name: ");
                            string fName = Console.ReadLine() ?? string.Empty;
                            await _studentService.UpdateStudentFirstName(studentId, fName);
                            break;
                        case 2:
                            Console.Write("Enter new last name: ");
                            string lName = Console.ReadLine() ?? string.Empty;
                            await _studentService.UpdateStudentLastName(studentId, lName);
                            break;
                        case 3:
                            Console.Write("Enter new email: ");
                            string email = Console.ReadLine() ?? string.Empty;
                            await _studentService.UpdateStudentEmail(studentId, email);
                            break;
                        case 4:
                            Console.Write("Enter new phone number: ");
                            string phone = Console.ReadLine() ?? string.Empty;
                            await _studentService.UpdateStudentPhoneNumber(studentId, phone);
                            break;
                        case 5:
                            Console.Write("Enter new college: ");
                            string college = Console.ReadLine() ?? string.Empty;
                            await _studentService.UpdateStudentCollege(studentId, college);
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
        public void ViewStudents(AppDbContext context)
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

                    IQueryable<Student> query = context.Students.AsQueryable();
                    List<Student> students;

                    switch (choice)
                    {
                        case 1:
                            students = StudentQueries
                                .GetAllStudent(query)
                                .ToList();
                            forLoop(students);
                            break;

                        case 2:
                            Console.Write("Enter First name: ");
                            string fName = Console.ReadLine() ?? string.Empty;
                            Console.Write("Enter Last name: ");
                            string lName = Console.ReadLine() ?? string.Empty;

                            students = StudentQueries
                                .GetStudentWithName(query, fName, lName)
                                .ToList();
                            forLoop(students);
                            break;

                        case 3:
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine() ?? string.Empty;

                            students = StudentQueries
                                .GetStudentWithEmail(query, email)
                                .ToList();
                            forLoop(students);
                            break;

                        case 4:
                            Console.Write("Enter Phone: ");
                            string phone = Console.ReadLine() ?? string.Empty;

                            students = StudentQueries
                                .GetStudentWithPhone(query, phone)
                                .ToList();
                            forLoop(students);
                            break;

                        case 5:
                            Console.Write("Enter College: ");
                            string college = Console.ReadLine() ?? string.Empty;

                            students = StudentQueries
                                .GetStudentWithCollege(query, college)
                                .ToList();
                            forLoop(students);
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
        private void forLoop(List<Student> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}

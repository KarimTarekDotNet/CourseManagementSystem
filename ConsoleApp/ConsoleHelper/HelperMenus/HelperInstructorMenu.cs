using Project.Data;
using Project.Entities;
using Project.Queries;
using Project.Service;

namespace ConsoleApp.ConsoleHelper.HelperMenus
{
    public class HelperInstructorMenu
    {
        private readonly InstructorService _instructorService;

        public HelperInstructorMenu(InstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public async Task AddInstructor()
        {
            try
            {
                Console.WriteLine("=== Add New Instructor ===");
                Console.Write("Enter Instructor First name: ");
                string fName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Instructor Last name: ");
                string lName = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Instructor Department (optional): ");
                string? department = Console.ReadLine();
                Console.Write("Enter Instructor Email: ");
                string email = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter Instructor Phone: ");
                string phone = Console.ReadLine() ?? string.Empty;
                await _instructorService.AddInstructor(fName, lName, department, email, phone);
                Console.WriteLine($"{fName + " " + lName} added successfully.");
                Console.WriteLine($"if you want your Id please search in view instructors -> view by name");
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
                Console.WriteLine("=== Delete Instructor ===");
                int instructorId = ReadIntInput("Enter Instructor Id: ");
                await _instructorService.RemoveInstructor(instructorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateInstructor()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("=== Update Instructor ===");
                    Console.WriteLine("1. update first name");
                    Console.WriteLine("2. update last name");
                    Console.WriteLine("3. update email");
                    Console.WriteLine("4. update phone");
                    Console.WriteLine("5. update department");
                    Console.WriteLine("0. return menu");
                    int choice = ReadIntInput("Select an option: ");
                    int instructorId = ReadIntInput("Enter Instructor Id: ");
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter new First name: ");
                            string fName = Console.ReadLine() ?? string.Empty;
                            await _instructorService.UpdateInstructorFirstName(instructorId, fName);
                            break;
                        case 2:
                            Console.Write("Enter new last name: ");
                            string lName = Console.ReadLine() ?? string.Empty;
                            await _instructorService.UpdateInstructorLastName(instructorId, lName);
                            break;
                        case 3:
                            Console.Write("Enter new email: ");
                            string email = Console.ReadLine() ?? string.Empty;
                            await _instructorService.UpdateInstructorEmail(instructorId, email);
                            break;
                        case 4:
                            Console.Write("Enter new phone number: ");
                            string phone = Console.ReadLine() ?? string.Empty;
                            await _instructorService.UpdateInstructorPhoneNumber(instructorId, phone);
                            break;
                        case 5:
                            Console.Write("Enter new department: ");
                            string department = Console.ReadLine() ?? string.Empty;
                            await _instructorService.UpdateInstructorDepartment(instructorId, department);
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

        public void ViewInstructors(AppDbContext context)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("=== View Instructors ===");
                    Console.WriteLine("1. View all instructors");
                    Console.WriteLine("2. View by name");
                    Console.WriteLine("3. View by email");
                    Console.WriteLine("4. View by department");
                    Console.WriteLine("5. View instructors with courses");
                    Console.WriteLine("6. View instructors without courses");
                    Console.WriteLine("0. Return menu");

                    int choice = ReadIntInput("Select an option: ");

                    IQueryable<Instructor> query = context.Instructors.AsQueryable();
                    List<Instructor> instructors;

                    switch (choice)
                    {
                        case 1:
                            instructors = InstructorQueries
                                .GetAllInstructors(query)
                                .ToList();

                            forLoop(instructors);
                            break;

                        case 2:
                            Console.Write("Enter First name: ");
                            string fName = Console.ReadLine() ?? string.Empty;
                            Console.Write("Enter Last name: ");
                            string lName = Console.ReadLine() ?? string.Empty;

                            instructors = InstructorQueries
                                .GetInstructorByName(query, fName, lName)
                                .ToList();
                            forLoop(instructors);
                            break;

                        case 3:
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine() ?? string.Empty;

                            instructors = InstructorQueries
                                .GetInstructorByEmail(query, email)
                                .ToList();
                            forLoop(instructors);
                            break;

                        case 4:
                            Console.Write("Enter Department: ");
                            string department = Console.ReadLine() ?? string.Empty;

                            instructors = InstructorQueries
                                .GetInstructorsByDepartment(query, department)
                                .ToList();
                            forLoop(instructors);
                            break;

                        case 5:
                            instructors = InstructorQueries
                                .GetInstructorsWithAnyCourses(query)
                                .ToList();
                            forLoop(instructors);
                            break;

                        case 6:
                            instructors = InstructorQueries
                                .GetInstructorsWithoutCourses(query)
                                .ToList();
                            forLoop(instructors);
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

        public async Task AssignCourse()
        {
            int instructorId = ReadIntInput("Enter Instructor ID: ");
            int courseId = ReadIntInput("Enter Course ID: ");

            try
            {
                await _instructorService.AssignCourse(instructorId, courseId);
                Console.WriteLine("Course assigned successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoveCourse()
        {
            int instructorId = ReadIntInput("Enter Instructor ID: ");
            int courseId = ReadIntInput("Enter Course ID: ");

            try
            {
                await _instructorService.RemoveCourse(instructorId, courseId);
                Console.WriteLine("Course removed successfully!");
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

        private void forLoop(List<Instructor> collection)
        {
            if(!collection.Any())
            {
                Console.WriteLine("No instructors found.");
                return;
            }
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
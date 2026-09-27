using EmployeeManagementSystemApp.Services;

namespace EmployeeManagementSystemApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the service
            EmployeeService employeeService = new EmployeeService();

            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   WELCOME TO EMPLOYEE MANAGEMENT SYSTEM APPLICATION   ║");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine("║        Designed for Junior Developers Learning        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            // Add some sample employees for demonstration
            Console.WriteLine("Loading sample data...\n");
            AddSampleData(employeeService);

            // Main application loop
            bool isRunning = true;

            while (isRunning)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewEmployee(employeeService);
                        break;

                    case "2":
                        employeeService.DisplayAllEmployees();
                        break;

                    case "3":
                        SearchEmployee(employeeService);
                        break;

                    case "4":
                        UpdateEmployeeInfo(employeeService);
                        break;

                    case "5":
                        DeleteEmployeeRecord(employeeService);
                        break;

                    case "6":
                        DisplayStatistics(employeeService);
                        break;

                    case "7":
                        SearchByDepartment(employeeService);
                        break;

                    case "8":
                        isRunning = false;
                        Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║       Thank you for using Employee Management System    ║");
                        Console.WriteLine("║                      Goodbye!                           ║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
                        break;

                    default:
                        Console.WriteLine("✗ Invalid choice! Please try again.\n");
                        break;
                }
            }
        }

        // Method to display the main menu
        static void DisplayMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    MAIN MENU                           ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Add New Employee                                   ║");
            Console.WriteLine("║  2. Display All Employees                              ║");
            Console.WriteLine("║  3. Search Employee                                    ║");
            Console.WriteLine("║  4. Update Employee Information                        ║");
            Console.WriteLine("║  5. Delete Employee                                    ║");
            Console.WriteLine("║  6. View Statistics                                    ║");
            Console.WriteLine("║  7. Search Employees by Department                     ║");
            Console.WriteLine("║  8. Exit Application                                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.Write("Enter your choice (1-8): ");
        }

        // Method to add a new employee
        static void AddNewEmployee(EmployeeService service)
        {
            Console.WriteLine("\n========== ADD NEW EMPLOYEE ==========");

            try
            {
                Console.Write("Enter First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Department: ");
                string department = Console.ReadLine();

                Console.Write("Enter Salary: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    service.AddEmployee(firstName, lastName, email, department, salary);
                }
                else
                {
                    Console.WriteLine("✗ Invalid salary amount. Please enter a valid number.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}\n");
            }
        }

        // Method to search employee
        static void SearchEmployee(EmployeeService service)
        {
            Console.WriteLine("\n========== SEARCH EMPLOYEE ==========");
            Console.WriteLine("1. Search by Employee ID");
            Console.WriteLine("2. Search by Name");
            Console.Write("Enter your choice (1-2): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Employee ID: ");
                    if (int.TryParse(Console.ReadLine(), out int employeeId))
                    {
                        service.SearchEmployeeById(employeeId);
                    }
                    else
                    {
                        Console.WriteLine("✗ Invalid Employee ID format.\n");
                    }
                    break;

                case "2":
                    Console.Write("Enter Name (First or Last): ");
                    string name = Console.ReadLine();
                    service.SearchEmployeeByName(name);
                    break;

                default:
                    Console.WriteLine("✗ Invalid choice!\n");
                    break;
            }
        }

        // Method to update employee information
        static void UpdateEmployeeInfo(EmployeeService service)
        {
            Console.WriteLine("\n========== UPDATE EMPLOYEE ==========");

            try
            {
                Console.Write("Enter Employee ID to update: ");
                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    Console.Write("Enter New First Name: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Enter New Last Name: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Enter New Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter New Department: ");
                    string department = Console.ReadLine();

                    Console.Write("Enter New Salary: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal salary))
                    {
                        service.UpdateEmployee(employeeId, firstName, lastName, email, department, salary);
                    }
                    else
                    {
                        Console.WriteLine("✗ Invalid salary amount.\n");
                    }
                }
                else
                {
                    Console.WriteLine("✗ Invalid Employee ID format.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}\n");
            }
        }

        // Method to delete employee
        static void DeleteEmployeeRecord(EmployeeService service)
        {
            Console.WriteLine("\n========== DELETE EMPLOYEE ==========");

            try
            {
                Console.Write("Enter Employee ID to delete: ");
                if (int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    Console.Write("Are you sure? (yes/no): ");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "yes" || confirmation == "y")
                    {
                        service.DeleteEmployee(employeeId);
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.\n");
                    }
                }
                else
                {
                    Console.WriteLine("✗ Invalid Employee ID format.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}\n");
            }
        }

        // Method to display statistics
        static void DisplayStatistics(EmployeeService service)
        {
            int totalEmployees = service.GetTotalEmployees();
            decimal avgSalary = service.GetAverageSalary();
            var highestPaid = service.GetHighestPaidEmployee();

            Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               EMPLOYEE STATISTICS                      ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║  Total Employees: {totalEmployees,-45}║");
            Console.WriteLine($"║  Average Salary: {avgSalary:C,-45}║");

            if (highestPaid != null)
            {
                Console.WriteLine($"║  Highest Paid: {highestPaid.GetFullName(),-43}║");
                Console.WriteLine($"║  Their Salary: {highestPaid.Salary:C,-44}║");
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        }

        // Method to search by department
        static void SearchByDepartment(EmployeeService service)
        {
            Console.WriteLine("\n========== SEARCH BY DEPARTMENT ==========");
            Console.Write("Enter Department Name: ");
            string department = Console.ReadLine();

            service.DisplayEmployeesByDepartment(department);
        }

        // Method to add sample data for testing
        static void AddSampleData(EmployeeService service)
        {
            service.AddEmployee("John", "Smith", "john.smith@company.com", "IT", 75000);
            service.AddEmployee("Sarah", "Johnson", "sarah.johnson@company.com", "HR", 65000);
            service.AddEmployee("Michael", "Williams", "michael.williams@company.com", "IT", 80000);
            service.AddEmployee("Emily", "Brown", "emily.brown@company.com", "Finance", 70000);
            service.AddEmployee("David", "Davis", "david.davis@company.com", "Sales", 60000);

            Console.WriteLine("✓ Sample data loaded successfully!\n");
        }
    }
}

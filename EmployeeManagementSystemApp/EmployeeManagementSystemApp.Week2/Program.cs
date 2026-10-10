using EmployeeManagementSystemApp.Week2.Exceptions;
using EmployeeManagementSystemApp.Week2.Interfaces;
using EmployeeManagementSystemApp.Week2.Models;
using EmployeeManagementSystemApp.Week2.Repositories;
using EmployeeManagementSystemApp.Week2.Services;

namespace EmployeeManagementSystemApp.Week2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEmployeeRepository repository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(repository);
            IEmployeeQueryService queryService = new EmployeeQueryService(repository);

            Console.WriteLine("Employee Management System - Week 2");
            Console.WriteLine("OOP, exceptions, and LINQ");
            Console.WriteLine();
            Console.WriteLine("Sample employees are loaded (IDs 1001-1006). Departments: IT, HR, Finance.");
            Console.WriteLine();

            bool isRunning = true;
            while (isRunning)
            {
                DisplayMenu();
                string? choice = Console.ReadLine();
                if (choice is null)
                    break;

                switch (choice.Trim())
                {
                    case "1":
                        RunOperation("Add Employee", () => AddEmployee(employeeService));
                        break;
                    case "2":
                        RunOperation("Update Employee", () => UpdateEmployee(employeeService));
                        break;
                    case "3":
                        RunOperation("Delete Employee", () => DeleteEmployee(employeeService));
                        break;
                    case "4":
                        RunOperation("Get Employee", () => GetEmployee(employeeService));
                        break;
                    case "5":
                        RunOperation("Get All Employees", () => GetAllEmployees(employeeService));
                        break;
                    case "6":
                        RunOperation("Search Employee", () => SearchEmployee(employeeService));
                        break;
                    case "7":
                        RunOperation("Filter Employees", () => FilterEmployees(employeeService, queryService));
                        break;
                    case "8":
                        RunOperation("Sort Employees", () => SortEmployees(employeeService, queryService));
                        break;
                    case "9":
                        RunOperation("Group Employees", () => GroupEmployees(queryService));
                        break;
                    case "10":
                        RunOperation("Salary Statistics", () => ShowSalaryStatistics(queryService));
                        break;
                    case "11":
                        RunOperation("Check Conditions", () => CheckConditions(queryService));
                        break;
                    case "12":
                        RunOperation("Join Employees and Departments", () => JoinEmployees(queryService));
                        break;
                    case "13":
                        isRunning = false;
                        Console.WriteLine("Goodbye.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 13.");
                        Console.WriteLine();
                        break;
                }
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("1.  Add Employee");
            Console.WriteLine("2.  Update Employee");
            Console.WriteLine("3.  Delete Employee");
            Console.WriteLine("4.  Get Employee");
            Console.WriteLine("5.  Get All Employees");
            Console.WriteLine("6.  Search Employee");
            Console.WriteLine("7.  Filter Employees (Where)");
            Console.WriteLine("8.  Sort Employees (OrderBy, ThenBy)");
            Console.WriteLine("9.  Group Employees by Department (GroupBy)");
            Console.WriteLine("10. Employee Salary Statistics (Count, Sum, Select)");
            Console.WriteLine("11. Check Employee Conditions (Any, All, First, FirstOrDefault, Single)");
            Console.WriteLine("12. Join Employees with Departments (Join)");
            Console.WriteLine("13. Exit");
            Console.Write("Enter your choice (1-13): ");
        }

        private static void RunOperation(string operationName, Action action)
        {
            try
            {
                action();
            }
            catch (EmployeeNotFoundException ex)
            {
                Console.WriteLine($"Employee not found: {ex.Message}");
            }
            catch (InvalidEmployeeException ex)
            {
                Console.WriteLine($"Invalid employee data: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Query result was not valid: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"--- {operationName} finished ---");
                Console.WriteLine();
            }
        }

        private static void AddEmployee(IEmployeeService employeeService)
        {
            PrintDepartments(employeeService);
            Console.WriteLine("Employee type: 1 = Developer, 2 = Manager");
            string type = ReadRequired("Type");
            string firstName = ReadRequired("First name");
            string lastName = ReadRequired("Last name");
            string email = ReadRequired("Email");
            string department = ReadRequired("Department");
            decimal salary = ReadDecimal("Salary");

            Employee employee;
            if (type == "1")
            {
                string language = ReadRequired("Programming language");
                employee = employeeService.AddDeveloper(firstName, lastName, email, department, salary, language);
            }
            else if (type == "2")
            {
                int teamSize = ReadInt("Team size");
                employee = employeeService.AddManager(firstName, lastName, email, department, salary, teamSize);
            }
            else
            {
                throw new InvalidEmployeeException("Type must be 1 for Developer or 2 for Manager.");
            }

            Console.WriteLine($"Added {employee.Role} {employee.GetFullName()} with ID {employee.EmployeeId}.");
            employee.DisplayDetails();
        }

        private static void UpdateEmployee(IEmployeeService employeeService)
        {
            int employeeId = ReadInt("Employee ID");
            Employee current = employeeService.GetEmployee(employeeId);
            Console.WriteLine("Current details:");
            current.DisplayDetails();

            PrintDepartments(employeeService);
            string firstName = ReadRequired("New first name");
            string lastName = ReadRequired("New last name");
            string email = ReadRequired("New email");
            string department = ReadRequired("New department");
            decimal salary = ReadDecimal("New salary");

            employeeService.UpdateEmployee(employeeId, firstName, lastName, email, department, salary);
            Console.WriteLine("Employee updated.");
            employeeService.GetEmployee(employeeId).DisplayDetails();
        }

        private static void DeleteEmployee(IEmployeeService employeeService)
        {
            int employeeId = ReadInt("Employee ID");
            Employee employee = employeeService.GetEmployee(employeeId);
            employee.DisplayDetails();
            Console.Write("Type yes to delete: ");
            string? confirmation = Console.ReadLine();
            if (!string.Equals(confirmation?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            employeeService.DeleteEmployee(employeeId);
            Console.WriteLine($"Employee {employeeId} deleted.");
        }

        private static void GetEmployee(IEmployeeService employeeService)
        {
            int employeeId = ReadInt("Employee ID");
            Employee employee = employeeService.GetEmployee(employeeId);
            employee.DisplayDetails();
        }

        private static void GetAllEmployees(IEmployeeService employeeService)
        {
            IReadOnlyList<Employee> employees = employeeService.GetAllEmployees();
            Console.WriteLine($"Employees: {employees.Count}");
            PrintEmployees(employeeService, employees);
        }

        private static void SearchEmployee(IEmployeeService employeeService)
        {
            string name = ReadRequired("Name");
            IReadOnlyList<Employee> matches = employeeService.SearchByName(name);
            if (matches.Count == 0)
            {
                Console.WriteLine($"No employee has a name containing '{name}'.");
                return;
            }

            PrintEmployees(employeeService, matches);
        }

        private static void FilterEmployees(IEmployeeService employeeService, IEmployeeQueryService queryService)
        {
            PrintDepartments(employeeService);
            string department = ReadRequired("Department");
            IReadOnlyList<Employee> matches = queryService.FilterByDepartment(department);
            Console.WriteLine($"Where: employees in {department}");
            if (matches.Count == 0)
            {
                Console.WriteLine("No employees matched. Where returns an empty list instead of throwing.");
                return;
            }

            PrintEmployees(employeeService, matches);
        }

        private static void SortEmployees(IEmployeeService employeeService, IEmployeeQueryService queryService)
        {
            Console.WriteLine("OrderBy department name, ThenBy salary:");
            PrintEmployees(employeeService, queryService.SortByDepartmentThenSalary());
        }

        private static void GroupEmployees(IEmployeeQueryService queryService)
        {
            Console.WriteLine("GroupBy department:");
            foreach (DepartmentEmployeeGroup group in queryService.GroupByDepartment())
            {
                Console.WriteLine();
                Console.WriteLine($"{group.DepartmentName} ({group.Employees.Count})");
                foreach (Employee employee in group.Employees)
                {
                    employee.DisplayDetails();
                }
            }
        }

        private static void ShowSalaryStatistics(IEmployeeQueryService queryService)
        {
            const decimal highSalary = 80000m;

            int total = queryService.CountEmployees();
            int highEarners = queryService.CountBySalaryAtLeast(highSalary);
            decimal payroll = queryService.SumSalaries();

            Console.WriteLine($"Count: {total} employees.");
            Console.WriteLine($"Count where salary >= {highSalary:C}: {highEarners}.");
            Console.WriteLine($"Sum of salaries: {payroll:C}.");
            Console.WriteLine();
            Console.WriteLine("Select: name, role, and salary only:");

            foreach (EmployeeNameSalary row in queryService.SelectNamesAndSalaries())
            {
                Console.WriteLine($"  {row.FullName} | {row.Role} | {row.Salary:C}");
            }
        }

        private static void CheckConditions(IEmployeeQueryService queryService)
        {
            decimal amount = ReadDecimal("Salary amount for Any and All");
            bool any = queryService.AnySalaryGreaterThan(amount);
            bool all = queryService.AllSalariesGreaterThan(amount);

            Console.WriteLine(any
                ? $"Any: at least one employee earns more than {amount:C}."
                : $"Any: no employee earns more than {amount:C}.");
            Console.WriteLine(all
                ? $"All: every employee earns more than {amount:C}."
                : $"All: not every employee earns more than {amount:C}.");

            string department = ReadRequired("Department for First and FirstOrDefault");
            try
            {
                Console.WriteLine($"First in {department}:");
                queryService.FindFirstInDepartment(department).DisplayDetails();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"First() threw because nobody is in '{department}'.");
            }

            Employee? firstOrDefault = queryService.FindFirstOrDefaultInDepartment(department);
            if (firstOrDefault is null)
                Console.WriteLine($"FirstOrDefault in '{department}' returned null and did not throw.");
            else
            {
                Console.WriteLine("FirstOrDefault found:");
                firstOrDefault.DisplayDetails();
            }

            Console.WriteLine("FirstOrDefault in 'Legal' (there is no Legal department):");
            Employee? missing = queryService.FindFirstOrDefaultInDepartment("Legal");
            if (missing is null)
                Console.WriteLine("Result is null. First() on the same department would throw.");

            try
            {
                queryService.FindSingleInDepartment("IT");
                Console.WriteLine("Single() unexpectedly found exactly one IT employee.");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Single() on IT threw because more than one employee is in IT.");
                Console.WriteLine("Single requires exactly one match. Zero matches would also throw.");
            }

            string email = ReadRequired("Email for Single");
            Console.WriteLine("Single by email:");
            queryService.FindSingleByEmail(email).DisplayDetails();
        }

        private static void JoinEmployees(IEmployeeQueryService queryService)
        {
            Console.WriteLine("Join employees with departments:");
            foreach (EmployeeDepartmentMatch match in queryService.JoinEmployeesWithDepartments())
            {
                Console.WriteLine(
                    $"{match.FullName} | {match.Role} | {match.DepartmentName} ({match.Location}) | {match.Salary:C}");
            }
        }

        private static void PrintEmployees(IEmployeeService employeeService, IReadOnlyList<Employee> employees)
        {
            foreach (Employee employee in employees)
            {
                string departmentName = FindDepartmentName(employeeService, employee.DepartmentId);
                Console.WriteLine($"[{departmentName}]");
                employee.DisplayDetails();
            }
        }

        private static void PrintDepartments(IEmployeeService employeeService)
        {
            Console.Write("Departments: ");
            IReadOnlyList<Department> departments = employeeService.GetDepartments();
            for (int index = 0; index < departments.Count; index++)
            {
                if (index > 0)
                    Console.Write(", ");
                Console.Write($"{departments[index].Name} ({departments[index].Location})");
            }

            Console.WriteLine();
        }

        private static string FindDepartmentName(IEmployeeService employeeService, int departmentId)
        {
            foreach (Department department in employeeService.GetDepartments())
            {
                if (department.DepartmentId == departmentId)
                    return department.Name;
            }

            return "Unknown";
        }

        private static string ReadRequired(string fieldName)
        {
            Console.Write($"{fieldName}: ");
            string? value = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmployeeException($"{fieldName} is required.");

            return value.Trim();
        }

        private static int ReadInt(string fieldName)
        {
            Console.Write($"{fieldName}: ");
            string? value = Console.ReadLine();
            if (!int.TryParse(value, out int number))
                throw new InvalidEmployeeException($"{fieldName} must be a whole number.");

            return number;
        }

        private static decimal ReadDecimal(string fieldName)
        {
            Console.Write($"{fieldName}: ");
            string? value = Console.ReadLine();
            if (!decimal.TryParse(value, out decimal number))
                throw new InvalidEmployeeException($"{fieldName} must be a number.");

            return number;
        }
    }
}

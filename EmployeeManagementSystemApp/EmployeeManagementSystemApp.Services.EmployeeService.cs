using EmployeeManagementSystemApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystemApp.Services
{
    /// <summary>
    /// EmployeeService handles all business logic for employee operations
    /// This class manages CRUD operations (Create, Read, Update, Delete)
    /// </summary>
    public class EmployeeService
    {
        private List<Employee> employees = new List<Employee>();
        private int nextEmployeeId = 1001;

        // Method to add a new employee
        public void AddEmployee(string firstName, string lastName, string email, string department, decimal salary)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(firstName)) firstName = "Unknown";
            if (string.IsNullOrWhiteSpace(lastName)) lastName = "Unknown";
            if (string.IsNullOrWhiteSpace(email)) email = "unknown@example.com";
            if (string.IsNullOrWhiteSpace(department)) department = "Unassigned";

            // Create a new employee object
            Employee employee = new Employee
            {
                EmployeeId = nextEmployeeId++,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Department = department,
                Salary = salary,
                JoiningDate = DateTime.Now
            };

            // Add to the list
            employees.Add(employee);
            Console.WriteLine($"? Employee {employee.GetFullName()} added successfully with ID: {employee.EmployeeId}\n");
        }

        // Method to display all employees
        public void DisplayAllEmployees()
        {
            // Check if there are any employees
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found in the system.\n");
                return;
            }

            Console.WriteLine("\n========== ALL EMPLOYEES ==========");
            Console.WriteLine("?????????????????????????????????????????????????????????????");

            // Loop through each employee and display info
            foreach (Employee emp in employees)
            {
                Console.WriteLine(emp.ToString());
            }

            Console.WriteLine("?????????????????????????????????????????????????????????????\n");
        }

        // Method to search employee by ID
        public void SearchEmployeeById(int employeeId)
        {
            // Use LINQ to find employee
            Employee? employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee != null)
            {
                Console.WriteLine("\n========== EMPLOYEE FOUND ==========");
                Console.WriteLine(employee.ToString());
                Console.WriteLine("????????????????????????????????\n");
            }
            else
            {
                Console.WriteLine($"? Employee with ID {employeeId} not found.\n");
            }
        }

        // Method to search employee by name
        public void SearchEmployeeByName(string? name)
        {
            // Handle null or empty name
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("? Please provide a valid name to search.\n");
                return;
            }

            // Use LINQ to find employees by first or last name
            List<Employee> results = employees
                .Where(e => e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
                           e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine($"? No employees found with name containing '{name}'.\n");
                return;
            }

            Console.WriteLine($"\n========== SEARCH RESULTS FOR '{name}' ==========");
            Console.WriteLine("??????????????????????????????????????????????????????????????");

            foreach (Employee emp in results)
            {
                Console.WriteLine(emp.ToString());
            }

            Console.WriteLine("??????????????????????????????????????????????????????????????\n");
        }

        // Method to update employee information
        public void UpdateEmployee(int employeeId, string? firstName, string? lastName, string? email, string? department, decimal salary)
        {
            // Find the employee
            Employee? employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee != null)
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    employee.FirstName = firstName;
                if (!string.IsNullOrWhiteSpace(lastName))
                    employee.LastName = lastName;
                if (!string.IsNullOrWhiteSpace(email))
                    employee.Email = email;
                if (!string.IsNullOrWhiteSpace(department))
                    employee.Department = department;

                employee.Salary = salary;

                Console.WriteLine($"? Employee with ID {employeeId} updated successfully.\n");
            }
            else
            {
                Console.WriteLine($"? Employee with ID {employeeId} not found.\n");
            }
        }

        // Method to delete an employee
        public void DeleteEmployee(int employeeId)
        {
            // Find and remove the employee
            Employee? employee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employee != null)
            {
                employees.Remove(employee);
                Console.WriteLine($"? Employee with ID {employeeId} deleted successfully.\n");
            }
            else
            {
                Console.WriteLine($"? Employee with ID {employeeId} not found.\n");
            }
        }

        // Method to get total number of employees
        public int GetTotalEmployees()
        {
            return employees.Count;
        }

        // Method to get average salary
        public decimal GetAverageSalary()
        {
            if (employees.Count == 0)
                return 0;

            return employees.Average(e => e.Salary);
        }

        // Method to get highest paid employee (nullable return)
        public Employee? GetHighestPaidEmployee()
        {
            if (employees.Count == 0)
                return null;

            return employees.OrderByDescending(e => e.Salary).FirstOrDefault();
        }

        // Method to get employees by department
        public void DisplayEmployeesByDepartment(string? department)
        {
            // Handle null or empty department
            if (string.IsNullOrWhiteSpace(department))
            {
                Console.WriteLine("? Please provide a valid department name.\n");
                return;
            }

            List<Employee> results = employees
                .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine($"? No employees found in department '{department}'.\n");
                return;
            }

            Console.WriteLine($"\n========== EMPLOYEES IN {department.ToUpper()} DEPARTMENT ==========");
            Console.WriteLine("??????????????????????????????????????????????????????????????");

            foreach (Employee emp in results)
            {
                Console.WriteLine(emp.ToString());
            }

            Console.WriteLine("??????????????????????????????????????????????????????????????\n");
        }
    }
}

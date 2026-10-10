using EmployeeManagementSystemApp.Week2.Exceptions;
using EmployeeManagementSystemApp.Week2.Interfaces;
using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees = new List<Employee>();
        private readonly List<Department> _departments = new List<Department>();
        private int _nextEmployeeId = 1001;

        public EmployeeRepository()
        {
            _departments.Add(new Department(1, "IT", "Hyderabad"));
            _departments.Add(new Department(2, "HR", "Bengaluru"));
            _departments.Add(new Department(3, "Finance", "Mumbai"));

            Add(new Developer(1001, "John", "Smith", "john.smith@company.com", 1, 75000m, "C#"));
            Add(new Manager(1002, "Sarah", "Johnson", "sarah.johnson@company.com", 2, 90000m, 6));
            Add(new Developer(1003, "Michael", "Williams", "michael.williams@company.com", 1, 80000m, "Java"));
            Add(new Manager(1004, "Priya", "Sharma", "priya.sharma@company.com", 1, 120000m, 8));
            Add(new Developer(1005, "Amit", "Patel", "amit.patel@company.com", 3, 70000m, "SQL"));
            Add(new Manager(1006, "David", "Lee", "david.lee@company.com", 3, 95000m, 4));
        }

        public IReadOnlyList<Employee> GetAll()
        {
            return _employees.ToList();
        }

        public IReadOnlyList<Department> GetDepartments()
        {
            return _departments.ToList();
        }

        public Department? FindDepartmentByName(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return null;

            foreach (Department department in _departments)
            {
                if (department.Name.Equals(departmentName.Trim(), StringComparison.OrdinalIgnoreCase))
                    return department;
            }

            return null;
        }

        public Employee GetById(int employeeId)
        {
            foreach (Employee employee in _employees)
            {
                if (employee.EmployeeId == employeeId)
                    return employee;
            }

            throw new EmployeeNotFoundException($"Employee with ID {employeeId} was not found.");
        }

        public void Add(Employee employee)
        {
            if (employee is null)
                throw new InvalidEmployeeException("Employee cannot be null.");

            foreach (Employee existing in _employees)
            {
                if (existing.EmployeeId == employee.EmployeeId)
                    throw new InvalidEmployeeException($"Employee ID {employee.EmployeeId} already exists.");
            }

            if (EmailExists(employee.Email, excludingEmployeeId: 0))
                throw new InvalidEmployeeException($"Email '{employee.Email}' is already used.");

            _employees.Add(employee);

            if (employee.EmployeeId >= _nextEmployeeId)
                _nextEmployeeId = employee.EmployeeId + 1;
        }

        public void Update(Employee employee)
        {
            if (employee is null)
                throw new InvalidEmployeeException("Employee cannot be null.");

            for (int index = 0; index < _employees.Count; index++)
            {
                if (_employees[index].EmployeeId == employee.EmployeeId)
                {
                    _employees[index] = employee;
                    return;
                }
            }

            throw new EmployeeNotFoundException($"Employee with ID {employee.EmployeeId} was not found.");
        }

        public void Delete(int employeeId)
        {
            Employee employee = GetById(employeeId);
            _employees.Remove(employee);
        }

        public int GetNextEmployeeId()
        {
            return _nextEmployeeId;
        }

        public bool EmailExists(string email, int excludingEmployeeId)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            foreach (Employee employee in _employees)
            {
                if (employee.EmployeeId != excludingEmployeeId &&
                    employee.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

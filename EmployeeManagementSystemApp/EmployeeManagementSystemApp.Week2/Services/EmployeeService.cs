using EmployeeManagementSystemApp.Week2.Exceptions;
using EmployeeManagementSystemApp.Week2.Interfaces;
using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Employee AddDeveloper(
            string firstName,
            string lastName,
            string email,
            string departmentName,
            decimal salary,
            string programmingLanguage)
        {
            Department department = GetRequiredDepartment(departmentName);
            int employeeId = _repository.GetNextEmployeeId();

            Developer developer = new Developer(
                employeeId,
                firstName,
                lastName,
                email,
                department.DepartmentId,
                salary,
                programmingLanguage);

            _repository.Add(developer);
            return developer;
        }

        public Employee AddManager(
            string firstName,
            string lastName,
            string email,
            string departmentName,
            decimal salary,
            int teamSize)
        {
            Department department = GetRequiredDepartment(departmentName);
            int employeeId = _repository.GetNextEmployeeId();

            Manager manager = new Manager(
                employeeId,
                firstName,
                lastName,
                email,
                department.DepartmentId,
                salary,
                teamSize);

            _repository.Add(manager);
            return manager;
        }

        public void UpdateEmployee(
            int employeeId,
            string firstName,
            string lastName,
            string email,
            string departmentName,
            decimal salary)
        {
            Employee employee = _repository.GetById(employeeId);
            Department department = GetRequiredDepartment(departmentName);

            if (_repository.EmailExists(email, employeeId))
                throw new InvalidEmployeeException($"Email '{email}' is already used by another employee.");

            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Email = email;
            employee.DepartmentId = department.DepartmentId;
            employee.Salary = salary;

            _repository.Update(employee);
        }

        public void DeleteEmployee(int employeeId)
        {
            _repository.Delete(employeeId);
        }

        public Employee GetEmployee(int employeeId)
        {
            return _repository.GetById(employeeId);
        }

        public IReadOnlyList<Employee> GetAllEmployees()
        {
            return _repository.GetAll();
        }

        public IReadOnlyList<Employee> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidEmployeeException("Please enter a name to search.");

            string term = name.Trim();
            List<Employee> matches = new List<Employee>();

            foreach (Employee employee in _repository.GetAll())
            {
                if (employee.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    employee.LastName.Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(employee);
                }
            }

            return matches;
        }

        public IReadOnlyList<Department> GetDepartments()
        {
            return _repository.GetDepartments();
        }

        private Department GetRequiredDepartment(string departmentName)
        {
            Department? department = _repository.FindDepartmentByName(departmentName);
            if (department is null)
            {
                throw new InvalidEmployeeException(
                    $"Department '{departmentName}' does not exist. Use IT, HR, or Finance.");
            }

            return department;
        }
    }
}

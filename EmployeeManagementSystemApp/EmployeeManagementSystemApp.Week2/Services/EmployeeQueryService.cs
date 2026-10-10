using EmployeeManagementSystemApp.Week2.Exceptions;
using EmployeeManagementSystemApp.Week2.Interfaces;
using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Services
{
    public class EmployeeQueryService : IEmployeeQueryService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeQueryService(IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IReadOnlyList<Employee> FilterByDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new InvalidEmployeeException("Department name is required.");

            string name = departmentName.Trim();
            List<Employee> matches = _repository.GetAll()
                .Where(employee => GetDepartmentName(employee.DepartmentId)
                    .Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return matches;
        }

        public IReadOnlyList<EmployeeNameSalary> SelectNamesAndSalaries()
        {
            List<EmployeeNameSalary> rows = _repository.GetAll()
                .Select(employee => new EmployeeNameSalary(employee.GetFullName(), employee.Role, employee.Salary))
                .ToList();

            return rows;
        }

        public IReadOnlyList<Employee> SortByDepartmentThenSalary()
        {
            List<Employee> sorted = _repository.GetAll()
                .OrderBy(employee => GetDepartmentName(employee.DepartmentId))
                .ThenBy(employee => employee.Salary)
                .ToList();

            return sorted;
        }

        public IReadOnlyList<DepartmentEmployeeGroup> GroupByDepartment()
        {
            List<DepartmentEmployeeGroup> groups = _repository.GetAll()
                .GroupBy(employee => GetDepartmentName(employee.DepartmentId))
                .OrderBy(group => group.Key)
                .Select(group => new DepartmentEmployeeGroup(group.Key, group.ToList()))
                .ToList();

            return groups;
        }

        public bool AnySalaryGreaterThan(decimal amount)
        {
            return _repository.GetAll().Any(employee => employee.Salary > amount);
        }

        public bool AllSalariesGreaterThan(decimal amount)
        {
            return _repository.GetAll().All(employee => employee.Salary > amount);
        }

        public Employee FindFirstInDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new InvalidEmployeeException("Department name is required.");

            string name = departmentName.Trim();
            return _repository.GetAll()
                .Where(employee => GetDepartmentName(employee.DepartmentId)
                    .Equals(name, StringComparison.OrdinalIgnoreCase))
                .First();
        }

        public Employee? FindFirstOrDefaultInDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new InvalidEmployeeException("Department name is required.");

            string name = departmentName.Trim();
            return _repository.GetAll()
                .Where(employee => GetDepartmentName(employee.DepartmentId)
                    .Equals(name, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        public Employee FindSingleByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidEmployeeException("Email is required.");

            string normalized = email.Trim();
            return _repository.GetAll()
                .Single(employee => employee.Email.Equals(normalized, StringComparison.OrdinalIgnoreCase));
        }

        public Employee FindSingleInDepartment(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new InvalidEmployeeException("Department name is required.");

            string name = departmentName.Trim();
            return _repository.GetAll()
                .Where(employee => GetDepartmentName(employee.DepartmentId)
                    .Equals(name, StringComparison.OrdinalIgnoreCase))
                .Single();
        }

        public int CountEmployees()
        {
            return _repository.GetAll().Count();
        }

        public int CountBySalaryAtLeast(decimal amount)
        {
            return _repository.GetAll().Count(employee => employee.Salary >= amount);
        }

        public decimal SumSalaries()
        {
            return _repository.GetAll().Sum(employee => employee.Salary);
        }

        public IReadOnlyList<EmployeeDepartmentMatch> JoinEmployeesWithDepartments()
        {
            IReadOnlyList<Employee> employees = _repository.GetAll();
            IReadOnlyList<Department> departments = _repository.GetDepartments();

            List<EmployeeDepartmentMatch> matches = employees
                .Join(
                    departments,
                    employee => employee.DepartmentId,
                    department => department.DepartmentId,
                    (employee, department) => new EmployeeDepartmentMatch(
                        employee.GetFullName(),
                        employee.Role,
                        employee.Salary,
                        department.Name,
                        department.Location))
                .OrderBy(match => match.DepartmentName)
                .ThenBy(match => match.FullName)
                .ToList();

            return matches;
        }

        private string GetDepartmentName(int departmentId)
        {
            foreach (Department department in _repository.GetDepartments())
            {
                if (department.DepartmentId == departmentId)
                    return department.Name;
            }

            throw new InvalidEmployeeException($"Department ID {departmentId} does not exist.");
        }
    }
}

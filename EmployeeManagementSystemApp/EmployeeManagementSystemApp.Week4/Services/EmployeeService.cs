using EmployeeManagementSystemApp.Week4.Dtos;
using EmployeeManagementSystemApp.Week4.Exceptions;
using EmployeeManagementSystemApp.Week4.Interfaces;
using EmployeeManagementSystemApp.Week4.Models;

namespace EmployeeManagementSystemApp.Week4.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository, ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<Employee> employees = await _repository.GetAllAsync(cancellationToken);
            return employees.Select(Map).ToList();
        }

        public async Task<EmployeeResponse> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployee(employeeId, cancellationToken);
            return Map(employee);
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            await EnsureDepartmentExists(request.DepartmentId, cancellationToken);
            await EnsureEmailIsFree(request.Email, 0, cancellationToken);

            Employee employee = new Employee
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                Salary = request.Salary,
                DepartmentId = request.DepartmentId,
                JoiningDate = DateTime.UtcNow
            };

            Employee created = await _repository.AddAsync(employee, cancellationToken);
            _logger.LogInformation("Created employee {EmployeeId}", created.EmployeeId);
            return Map(created);
        }

        public async Task<EmployeeResponse> UpdateAsync(int employeeId, UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployee(employeeId, cancellationToken);
            await EnsureDepartmentExists(request.DepartmentId, cancellationToken);
            await EnsureEmailIsFree(request.Email, employeeId, cancellationToken);

            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();
            employee.Email = request.Email.Trim();
            employee.Salary = request.Salary;
            employee.DepartmentId = request.DepartmentId;

            await _repository.UpdateAsync(employee, cancellationToken);
            _logger.LogInformation("Updated employee {EmployeeId}", employeeId);
            return Map(employee);
        }

        public async Task<EmployeeResponse> PatchAsync(int employeeId, PatchEmployeeRequest request, CancellationToken cancellationToken)
        {
            if (request.FirstName is null &&
                request.LastName is null &&
                request.Email is null &&
                request.Salary is null &&
                request.DepartmentId is null)
            {
                throw new InvalidEmployeeException("At least one field is required for PATCH.");
            }

            Employee employee = await GetRequiredEmployee(employeeId, cancellationToken);

            if (request.DepartmentId.HasValue)
                await EnsureDepartmentExists(request.DepartmentId.Value, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Email))
                await EnsureEmailIsFree(request.Email, employeeId, cancellationToken);

            if (request.FirstName is not null)
                employee.FirstName = request.FirstName.Trim();
            if (request.LastName is not null)
                employee.LastName = request.LastName.Trim();
            if (request.Email is not null)
                employee.Email = request.Email.Trim();
            if (request.Salary.HasValue)
                employee.Salary = request.Salary.Value;
            if (request.DepartmentId.HasValue)
                employee.DepartmentId = request.DepartmentId.Value;

            await _repository.UpdateAsync(employee, cancellationToken);
            _logger.LogInformation("Patched employee {EmployeeId}", employeeId);
            return Map(employee);
        }

        public async Task DeleteAsync(int employeeId, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployee(employeeId, cancellationToken);
            await _repository.DeleteAsync(employee, cancellationToken);
            _logger.LogInformation("Deleted employee {EmployeeId}", employeeId);
        }

        public async Task<IReadOnlyList<DepartmentResponse>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<Department> departments = await _repository.GetDepartmentsAsync(cancellationToken);
            return departments
                .Select(department => new DepartmentResponse
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                    Location = department.Location
                })
                .ToList();
        }

        private async Task<Employee> GetRequiredEmployee(int employeeId, CancellationToken cancellationToken)
        {
            Employee? employee = await _repository.GetByIdAsync(employeeId, cancellationToken);
            if (employee is null)
                throw new EmployeeNotFoundException($"Employee with ID {employeeId} was not found.");

            return employee;
        }

        private async Task EnsureDepartmentExists(int departmentId, CancellationToken cancellationToken)
        {
            bool exists = await _repository.DepartmentExistsAsync(departmentId, cancellationToken);
            if (!exists)
                throw new InvalidEmployeeException($"Department ID {departmentId} does not exist.");
        }

        private async Task EnsureEmailIsFree(string email, int excludingEmployeeId, CancellationToken cancellationToken)
        {
            bool exists = await _repository.EmailExistsAsync(email.Trim(), excludingEmployeeId, cancellationToken);
            if (exists)
                throw new InvalidEmployeeException($"Email '{email}' is already used.");
        }

        private static EmployeeResponse Map(Employee employee)
        {
            return new EmployeeResponse
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = employee.Salary,
                JoiningDate = employee.JoiningDate,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? string.Empty
            };
        }
    }
}

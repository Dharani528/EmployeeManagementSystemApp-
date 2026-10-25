using AutoMapper;
using EmployeeManagementSystemApp.Week5.Dtos;
using EmployeeManagementSystemApp.Week5.Exceptions;
using EmployeeManagementSystemApp.Week5.Interfaces;
using EmployeeManagementSystemApp.Week5.Mapping;
using EmployeeManagementSystemApp.Week5.Models;

namespace EmployeeManagementSystemApp.Week5.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _repository.GetProjectedEmployeesAsync(cancellationToken);
        }

        public async Task<EmployeeResponse> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployeeRead(employeeId, cancellationToken);
            return _mapper.Map<EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            await EnsureDepartmentExists(request.DepartmentId, cancellationToken);
            await EnsureEmailIsFree(request.Email, 0, cancellationToken);

            Employee employee = EmployeeMapper.ToEntity(request);

            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction =
                await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                Employee created = await _repository.AddAsync(employee, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Created employee {EmployeeId} with {SkillCount} skills", created.EmployeeId, created.Skills.Count);
                return await GetByIdAsync(created.EmployeeId, cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<EmployeeResponse> UpdateAsync(int employeeId, UpdateEmployeeRequest request, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployeeTracked(employeeId, cancellationToken);
            await EnsureDepartmentExists(request.DepartmentId, cancellationToken);
            await EnsureEmailIsFree(request.Email, employeeId, cancellationToken);

            EmployeeMapper.ApplyUpdate(employee, request);
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Updated employee {EmployeeId}", employeeId);
            return await GetByIdAsync(employeeId, cancellationToken);
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

            Employee employee = await GetRequiredEmployeeTracked(employeeId, cancellationToken);

            if (request.DepartmentId.HasValue)
                await EnsureDepartmentExists(request.DepartmentId.Value, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Email))
                await EnsureEmailIsFree(request.Email, employeeId, cancellationToken);

            EmployeeMapper.ApplyPatch(employee, request);
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Patched employee {EmployeeId}", employeeId);
            return await GetByIdAsync(employeeId, cancellationToken);
        }

        public async Task DeleteAsync(int employeeId, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployeeTracked(employeeId, cancellationToken);
            await _repository.DeleteAsync(employee, cancellationToken);
            _logger.LogInformation("Deleted employee {EmployeeId}", employeeId);
        }

        public Task<IReadOnlyList<DepartmentResponse>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            return _repository.GetProjectedDepartmentsAsync(cancellationToken);
        }

        public async Task<DepartmentDetailResponse> GetDepartmentByIdAsync(int departmentId, CancellationToken cancellationToken)
        {
            Department? department = await _repository.GetDepartmentWithEmployeesAsync(departmentId, cancellationToken);
            if (department is null)
                throw new InvalidEmployeeException($"Department ID {departmentId} does not exist.");

            return _mapper.Map<DepartmentDetailResponse>(department);
        }

        public Task<IReadOnlyList<SalaryByDepartmentResponse>> GetSalaryByDepartmentAsync(CancellationToken cancellationToken)
        {
            return _repository.GetSalaryByDepartmentAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<EmployeeSummaryResponse>> GiveDepartmentRaiseAsync(
            int departmentId,
            GiveRaiseRequest request,
            CancellationToken cancellationToken)
        {
            await EnsureDepartmentExists(departmentId, cancellationToken);

            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction =
                await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                IReadOnlyList<Employee> employees = await _repository.GetTrackedByDepartmentAsync(departmentId, cancellationToken);
                foreach (Employee employee in employees)
                {
                    employee.Salary = Math.Round(employee.Salary * (1 + (request.Percent / 100m)), 2);
                }

                await _repository.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Gave {Percent}% raise to department {DepartmentId}", request.Percent, departmentId);
                return _mapper.Map<List<EmployeeSummaryResponse>>(employees);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task<Employee> GetRequiredEmployeeRead(int employeeId, CancellationToken cancellationToken)
        {
            Employee? employee = await _repository.GetByIdWithDetailsAsync(employeeId, cancellationToken);
            if (employee is null)
                throw new EmployeeNotFoundException($"Employee with ID {employeeId} was not found.");

            return employee;
        }

        private async Task<Employee> GetRequiredEmployeeTracked(int employeeId, CancellationToken cancellationToken)
        {
            Employee? employee = await _repository.GetByIdTrackedAsync(employeeId, cancellationToken);
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
    }
}

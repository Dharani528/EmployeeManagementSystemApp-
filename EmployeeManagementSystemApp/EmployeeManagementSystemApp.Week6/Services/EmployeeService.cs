using System.Security.Claims;
using AutoMapper;
using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Exceptions;
using EmployeeManagementSystemApp.Week6.Interfaces;
using EmployeeManagementSystemApp.Week6.Mapping;
using EmployeeManagementSystemApp.Week6.Models;

namespace EmployeeManagementSystemApp.Week6.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(
            IEmployeeRepository repository,
            IMapper mapper,
            ILogger<EmployeeService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<EmployeeResponse>> GetPagedAsync(
            EmployeeQueryParameters query,
            CancellationToken cancellationToken)
        {
            (IReadOnlyList<EmployeeResponse> items, int totalCount) =
                await _repository.GetPagedEmployeesAsync(query, cancellationToken);

            int totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            return new PagedResponse<EmployeeResponse>
            {
                Success = true,
                Message = "Employees retrieved.",
                Data = items,
                Pagination = new PaginationMeta
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            };
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

            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction =
                await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                Employee created = await _repository.AddAsync(employee, cancellationToken);
                if (transaction is not null)
                    await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation(
                    "User {UserName} created employee {EmployeeId} with {SkillCount} skills",
                    CurrentUserName(),
                    created.EmployeeId,
                    created.Skills.Count);
                return await GetByIdAsync(created.EmployeeId, cancellationToken);
            }
            catch
            {
                if (transaction is not null)
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
            _logger.LogInformation("User {UserName} updated employee {EmployeeId}", CurrentUserName(), employeeId);
            return await GetByIdAsync(employeeId, cancellationToken);
        }

        public async Task<EmployeeResponse> PatchAsync(int employeeId, PatchEmployeeRequest request, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployeeTracked(employeeId, cancellationToken);

            if (request.DepartmentId.HasValue)
                await EnsureDepartmentExists(request.DepartmentId.Value, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Email))
                await EnsureEmailIsFree(request.Email, employeeId, cancellationToken);

            EmployeeMapper.ApplyPatch(employee, request);
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User {UserName} patched employee {EmployeeId}", CurrentUserName(), employeeId);
            return await GetByIdAsync(employeeId, cancellationToken);
        }

        public async Task DeleteAsync(int employeeId, CancellationToken cancellationToken)
        {
            Employee employee = await GetRequiredEmployeeTracked(employeeId, cancellationToken);
            await _repository.DeleteAsync(employee, cancellationToken);
            _logger.LogInformation("User {UserName} deleted employee {EmployeeId}", CurrentUserName(), employeeId);
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

            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction =
                await _repository.BeginTransactionAsync(cancellationToken);
            try
            {
                IReadOnlyList<Employee> employees = await _repository.GetTrackedByDepartmentAsync(departmentId, cancellationToken);
                foreach (Employee employee in employees)
                {
                    employee.Salary = Math.Round(employee.Salary * (1 + (request.Percent / 100m)), 2);
                }

                await _repository.SaveChangesAsync(cancellationToken);
                if (transaction is not null)
                    await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation(
                    "User {UserName} gave {Percent}% raise to department {DepartmentId}",
                    CurrentUserName(),
                    request.Percent,
                    departmentId);
                return _mapper.Map<List<EmployeeSummaryResponse>>(employees);
            }
            catch
            {
                if (transaction is not null)
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

        private string CurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.Name
                ?? _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
                ?? "anonymous";
        }
    }
}

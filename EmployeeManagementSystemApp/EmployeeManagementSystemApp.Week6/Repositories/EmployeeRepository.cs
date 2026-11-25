using EmployeeManagementSystemApp.Week6.Data;
using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Interfaces;
using EmployeeManagementSystemApp.Week6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagementSystemApp.Week6.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(IReadOnlyList<EmployeeResponse> Items, int TotalCount)> GetPagedEmployeesAsync(
            EmployeeQueryParameters query,
            CancellationToken cancellationToken)
        {
            IQueryable<Employee> employees = _dbContext.Employees.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string search = query.Search.Trim();
                employees = employees.Where(employee =>
                    employee.FirstName.Contains(search) ||
                    employee.LastName.Contains(search) ||
                    employee.Email.Contains(search));
            }

            if (query.DepartmentId.HasValue)
                employees = employees.Where(employee => employee.DepartmentId == query.DepartmentId.Value);

            if (query.MinSalary.HasValue)
                employees = employees.Where(employee => employee.Salary >= query.MinSalary.Value);

            if (query.MaxSalary.HasValue)
                employees = employees.Where(employee => employee.Salary <= query.MaxSalary.Value);

            int totalCount = await employees.CountAsync(cancellationToken);
            employees = ApplySort(employees, query.SortBy, query.SortDir);

            List<EmployeeResponse> items = await employees
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(employee => new EmployeeResponse
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    JoiningDate = employee.JoiningDate,
                    DepartmentId = employee.DepartmentId,
                    Department = new DepartmentSummaryResponse
                    {
                        DepartmentId = employee.Department!.DepartmentId,
                        Name = employee.Department.Name,
                        Location = employee.Department.Location
                    },
                    Skills = employee.Skills
                        .OrderBy(skill => skill.SkillId)
                        .Select(skill => new SkillResponse
                        {
                            SkillId = skill.SkillId,
                            Name = skill.Name,
                            Years = skill.Years
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Employee?> GetByIdTrackedAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .Include(employee => employee.Department)
                .Include(employee => employee.Skills)
                .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId, cancellationToken);
        }

        public async Task<Employee?> GetByIdWithDetailsAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .Include(employee => employee.Department)
                .Include(employee => employee.Skills)
                .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId, cancellationToken);
        }

        public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return employee;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, int excludingEmployeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .AnyAsync(employee => employee.Email == email && employee.EmployeeId != excludingEmployeeId, cancellationToken);
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments
                .AsNoTracking()
                .AnyAsync(department => department.DepartmentId == departmentId, cancellationToken);
        }

        public async Task<IReadOnlyList<DepartmentResponse>> GetProjectedDepartmentsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Departments
                .AsNoTracking()
                .OrderBy(department => department.DepartmentId)
                .Select(department => new DepartmentResponse
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                    Location = department.Location,
                    EmployeeCount = department.Employees.Count
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Department?> GetDepartmentWithEmployeesAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments
                .AsNoTracking()
                .Include(department => department.Employees)
                .FirstOrDefaultAsync(department => department.DepartmentId == departmentId, cancellationToken);
        }

        public async Task<IReadOnlyList<SalaryByDepartmentResponse>> GetSalaryByDepartmentAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .GroupBy(employee => employee.Department!.Name)
                .Select(group => new SalaryByDepartmentResponse
                {
                    DepartmentName = group.Key,
                    EmployeeCount = group.Count(),
                    TotalSalary = group.Sum(employee => employee.Salary),
                    AverageSalary = group.Average(employee => employee.Salary)
                })
                .OrderBy(row => row.DepartmentName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Employee>> GetTrackedByDepartmentAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .Where(employee => employee.DepartmentId == departmentId)
                .OrderBy(employee => employee.EmployeeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (!_dbContext.Database.IsRelational())
                return null;

            return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task<AppUser?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.UserName == userName, cancellationToken);
        }

        private static IQueryable<Employee> ApplySort(IQueryable<Employee> employees, string sortBy, string sortDir)
        {
            bool descending = sortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return sortBy.Trim().ToLowerInvariant() switch
            {
                "firstname" => descending ? employees.OrderByDescending(employee => employee.FirstName) : employees.OrderBy(employee => employee.FirstName),
                "lastname" => descending ? employees.OrderByDescending(employee => employee.LastName) : employees.OrderBy(employee => employee.LastName),
                "email" => descending ? employees.OrderByDescending(employee => employee.Email) : employees.OrderBy(employee => employee.Email),
                "salary" => descending ? employees.OrderByDescending(employee => employee.Salary) : employees.OrderBy(employee => employee.Salary),
                "joiningdate" => descending ? employees.OrderByDescending(employee => employee.JoiningDate) : employees.OrderBy(employee => employee.JoiningDate),
                "departmentid" => descending ? employees.OrderByDescending(employee => employee.DepartmentId) : employees.OrderBy(employee => employee.DepartmentId),
                _ => descending ? employees.OrderByDescending(employee => employee.EmployeeId) : employees.OrderBy(employee => employee.EmployeeId)
            };
        }
    }
}

using EmployeeManagementSystemApp.Week4.Data;
using EmployeeManagementSystemApp.Week4.Interfaces;
using EmployeeManagementSystemApp.Week4.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemApp.Week4.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .Include(employee => employee.Department)
                .OrderBy(employee => employee.EmployeeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .Include(employee => employee.Department)
                .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId, cancellationToken);
        }

        public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.Entry(employee).Reference(item => item.Department).LoadAsync(cancellationToken);
            return employee;
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.Entry(employee).Reference(item => item.Department).LoadAsync(cancellationToken);
        }

        public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, int excludingEmployeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees.AnyAsync(
                employee => employee.Email == email && employee.EmployeeId != excludingEmployeeId,
                cancellationToken);
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _dbContext.Departments.AnyAsync(department => department.DepartmentId == departmentId, cancellationToken);
        }

        public async Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Departments
                .AsNoTracking()
                .OrderBy(department => department.DepartmentId)
                .ToListAsync(cancellationToken);
        }
    }
}

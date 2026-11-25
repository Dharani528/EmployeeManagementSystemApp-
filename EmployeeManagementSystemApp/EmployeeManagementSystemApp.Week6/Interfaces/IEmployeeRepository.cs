using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagementSystemApp.Week6.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<(IReadOnlyList<EmployeeResponse> Items, int TotalCount)> GetPagedEmployeesAsync(
            EmployeeQueryParameters query,
            CancellationToken cancellationToken);
        Task<Employee?> GetByIdTrackedAsync(int employeeId, CancellationToken cancellationToken);
        Task<Employee?> GetByIdWithDetailsAsync(int employeeId, CancellationToken cancellationToken);
        Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task DeleteAsync(Employee employee, CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, int excludingEmployeeId, CancellationToken cancellationToken);
        Task<bool> DepartmentExistsAsync(int departmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DepartmentResponse>> GetProjectedDepartmentsAsync(CancellationToken cancellationToken);
        Task<Department?> GetDepartmentWithEmployeesAsync(int departmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<SalaryByDepartmentResponse>> GetSalaryByDepartmentAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<Employee>> GetTrackedByDepartmentAsync(int departmentId, CancellationToken cancellationToken);
        Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken);
        Task<AppUser?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken);
    }
}

using EmployeeManagementSystemApp.Week4.Models;

namespace EmployeeManagementSystemApp.Week4.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken);
        Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken);
        Task UpdateAsync(Employee employee, CancellationToken cancellationToken);
        Task DeleteAsync(Employee employee, CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, int excludingEmployeeId, CancellationToken cancellationToken);
        Task<bool> DepartmentExistsAsync(int departmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken);
    }
}

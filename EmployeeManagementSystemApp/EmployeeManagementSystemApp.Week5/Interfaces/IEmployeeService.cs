using EmployeeManagementSystemApp.Week5.Dtos;

namespace EmployeeManagementSystemApp.Week5.Interfaces
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<EmployeeResponse> GetByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken);
        Task<EmployeeResponse> UpdateAsync(int employeeId, UpdateEmployeeRequest request, CancellationToken cancellationToken);
        Task<EmployeeResponse> PatchAsync(int employeeId, PatchEmployeeRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(int employeeId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DepartmentResponse>> GetDepartmentsAsync(CancellationToken cancellationToken);
        Task<DepartmentDetailResponse> GetDepartmentByIdAsync(int departmentId, CancellationToken cancellationToken);
        Task<IReadOnlyList<SalaryByDepartmentResponse>> GetSalaryByDepartmentAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<EmployeeSummaryResponse>> GiveDepartmentRaiseAsync(int departmentId, GiveRaiseRequest request, CancellationToken cancellationToken);
    }
}

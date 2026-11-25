using EmployeeManagementSystemApp.Week6.Dtos;

namespace EmployeeManagementSystemApp.Week6.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResponse<EmployeeResponse>> GetPagedAsync(EmployeeQueryParameters query, CancellationToken cancellationToken);
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

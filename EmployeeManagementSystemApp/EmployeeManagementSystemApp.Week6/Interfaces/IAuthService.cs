using EmployeeManagementSystemApp.Week6.Dtos;

namespace EmployeeManagementSystemApp.Week6.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}

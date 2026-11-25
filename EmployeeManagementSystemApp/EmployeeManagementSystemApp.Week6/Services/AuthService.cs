using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Exceptions;
using EmployeeManagementSystemApp.Week6.Interfaces;
using EmployeeManagementSystemApp.Week6.Models;
using EmployeeManagementSystemApp.Week6.Security;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystemApp.Week6.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _repository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly PasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();

        public AuthService(IEmployeeRepository repository, JwtTokenService jwtTokenService, ILogger<AuthService> logger)
        {
            _repository = repository;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _repository.FindUserByUserNameAsync(request.UserName.Trim(), cancellationToken);
            if (user is null || !user.IsActive)
            {
                _logger.LogWarning("Login failed for user {UserName}", request.UserName);
                throw new InvalidCredentialsException("Invalid user name or password.");
            }

            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed for user {UserName}", user.UserName);
                throw new InvalidCredentialsException("Invalid user name or password.");
            }

            (string token, DateTime expiresAtUtc) = _jwtTokenService.CreateToken(user);
            _logger.LogInformation("User {UserName} signed in with role {Role}", user.UserName, user.Role);

            return new LoginResponse
            {
                AccessToken = token,
                ExpiresAtUtc = expiresAtUtc,
                UserName = user.UserName,
                Role = user.Role,
                DisplayName = user.DisplayName
            };
        }
    }
}

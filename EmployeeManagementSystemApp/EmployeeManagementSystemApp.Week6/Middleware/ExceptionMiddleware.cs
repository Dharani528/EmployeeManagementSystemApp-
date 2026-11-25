using System.Net;
using System.Text.Json;
using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Exceptions;

namespace EmployeeManagementSystemApp.Week6.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogWarning(ex, "Employee was not found. TraceId {TraceId}", context.TraceIdentifier);
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (InvalidEmployeeException ex)
            {
                _logger.LogWarning(ex, "Invalid employee data. TraceId {TraceId}", context.TraceIdentifier);
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning("Login failed. TraceId {TraceId}", context.TraceIdentifier);
                await WriteError(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (OperationCanceledException)
            {
                context.Response.StatusCode = 499;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error. TraceId {TraceId}", context.TraceIdentifier);
                await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            ApiResponse<object> body = ApiResponse<object>.Fail(message, new[] { message }, context.TraceIdentifier);
            await context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
        }
    }
}

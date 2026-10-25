using System.Net;
using System.Text.Json;
using EmployeeManagementSystemApp.Week5.Exceptions;

namespace EmployeeManagementSystemApp.Week5.Middleware
{
    public class ExceptionMiddleware
    {
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
                _logger.LogWarning(ex, "Employee was not found.");
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (InvalidEmployeeException ex)
            {
                _logger.LogWarning(ex, "Invalid employee data.");
                await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (OperationCanceledException)
            {
                context.Response.StatusCode = 499;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error.");
                await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            string body = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(body);
        }
    }
}

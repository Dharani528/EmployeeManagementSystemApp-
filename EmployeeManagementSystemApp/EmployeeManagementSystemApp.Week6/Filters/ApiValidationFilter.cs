using EmployeeManagementSystemApp.Week6.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagementSystemApp.Week6.Filters
{
    public class ApiValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            List<string> errors = context.ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "Invalid value." : error.ErrorMessage)
                .ToList();

            ApiResponse<object> body = ApiResponse<object>.Fail(
                "Validation failed.",
                errors,
                context.HttpContext.TraceIdentifier);

            context.Result = new BadRequestObjectResult(body);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

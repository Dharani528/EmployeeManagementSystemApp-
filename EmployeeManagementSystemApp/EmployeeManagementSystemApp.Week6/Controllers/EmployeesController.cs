using Asp.Versioning;
using EmployeeManagementSystemApp.Week6.Dtos;
using EmployeeManagementSystemApp.Week6.Interfaces;
using EmployeeManagementSystemApp.Week6.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemApp.Week6.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Read)]
        [ProducesResponseType(typeof(PagedResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<EmployeeResponse>>> GetAll(
            [FromQuery] EmployeeQueryParameters query,
            CancellationToken cancellationToken)
        {
            PagedResponse<EmployeeResponse> result = await _employeeService.GetPagedAsync(query, cancellationToken);
            result.TraceId = HttpContext.TraceIdentifier;
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.Read)]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetById(int id, CancellationToken cancellationToken)
        {
            EmployeeResponse employee = await _employeeService.GetByIdAsync(id, cancellationToken);
            return Ok(ApiResponse<EmployeeResponse>.Ok(employee, "Employee retrieved.", HttpContext.TraceIdentifier));
        }

        [HttpGet("reports/salary-by-department")]
        [Authorize(Roles = AppRoles.Read)]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SalaryByDepartmentResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<SalaryByDepartmentResponse>>>> GetSalaryByDepartment(
            CancellationToken cancellationToken)
        {
            IReadOnlyList<SalaryByDepartmentResponse> rows = await _employeeService.GetSalaryByDepartmentAsync(cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<SalaryByDepartmentResponse>>.Ok(rows, "Salary report retrieved.", HttpContext.TraceIdentifier));
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.WriteEmployees)]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Create(
            [FromBody] CreateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse created = await _employeeService.CreateAsync(request, cancellationToken);
            ApiResponse<EmployeeResponse> body = ApiResponse<EmployeeResponse>.Ok(created, "Employee created.", HttpContext.TraceIdentifier);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId, version = "1.0" }, body);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.WriteEmployees)]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Update(
            int id,
            [FromBody] UpdateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse updated = await _employeeService.UpdateAsync(id, request, cancellationToken);
            return Ok(ApiResponse<EmployeeResponse>.Ok(updated, "Employee updated.", HttpContext.TraceIdentifier));
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = AppRoles.WriteEmployees)]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Patch(
            int id,
            [FromBody] PatchEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse patched = await _employeeService.PatchAsync(id, request, cancellationToken);
            return Ok(ApiResponse<EmployeeResponse>.Ok(patched, "Employee patched.", HttpContext.TraceIdentifier));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.WriteEmployees)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}

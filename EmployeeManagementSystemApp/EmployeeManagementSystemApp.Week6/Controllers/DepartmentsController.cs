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
    [Route("api/v{version:apiVersion}/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public DepartmentsController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Read)]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<DepartmentResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<DepartmentResponse>>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyList<DepartmentResponse> departments = await _employeeService.GetDepartmentsAsync(cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<DepartmentResponse>>.Ok(departments, "Departments retrieved.", HttpContext.TraceIdentifier));
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.Read)]
        [ProducesResponseType(typeof(ApiResponse<DepartmentDetailResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<DepartmentDetailResponse>>> GetById(int id, CancellationToken cancellationToken)
        {
            DepartmentDetailResponse department = await _employeeService.GetDepartmentByIdAsync(id, cancellationToken);
            return Ok(ApiResponse<DepartmentDetailResponse>.Ok(department, "Department retrieved.", HttpContext.TraceIdentifier));
        }

        [HttpPost("{id:int}/raise")]
        [Authorize(Roles = AppRoles.GiveRaise)]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EmployeeSummaryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<EmployeeSummaryResponse>>>> GiveRaise(
            int id,
            [FromBody] GiveRaiseRequest request,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<EmployeeSummaryResponse> employees =
                await _employeeService.GiveDepartmentRaiseAsync(id, request, cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<EmployeeSummaryResponse>>.Ok(employees, "Department raise applied.", HttpContext.TraceIdentifier));
        }
    }
}

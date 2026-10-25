using EmployeeManagementSystemApp.Week5.Dtos;
using EmployeeManagementSystemApp.Week5.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemApp.Week5.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public DepartmentsController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<DepartmentResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<DepartmentResponse>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyList<DepartmentResponse> departments = await _employeeService.GetDepartmentsAsync(cancellationToken);
            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DepartmentDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartmentDetailResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            DepartmentDetailResponse department = await _employeeService.GetDepartmentByIdAsync(id, cancellationToken);
            return Ok(department);
        }

        [HttpPost("{id:int}/raise")]
        [ProducesResponseType(typeof(IReadOnlyList<EmployeeSummaryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EmployeeSummaryResponse>>> GiveRaise(
            int id,
            [FromBody] GiveRaiseRequest request,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<EmployeeSummaryResponse> employees =
                await _employeeService.GiveDepartmentRaiseAsync(id, request, cancellationToken);
            return Ok(employees);
        }
    }
}

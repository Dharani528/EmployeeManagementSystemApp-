using EmployeeManagementSystemApp.Week5.Dtos;
using EmployeeManagementSystemApp.Week5.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemApp.Week5.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<EmployeeResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<EmployeeResponse>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyList<EmployeeResponse> employees = await _employeeService.GetAllAsync(cancellationToken);
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            EmployeeResponse employee = await _employeeService.GetByIdAsync(id, cancellationToken);
            return Ok(employee);
        }

        [HttpGet("reports/salary-by-department")]
        [ProducesResponseType(typeof(IReadOnlyList<SalaryByDepartmentResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<SalaryByDepartmentResponse>>> GetSalaryByDepartment(
            CancellationToken cancellationToken)
        {
            IReadOnlyList<SalaryByDepartmentResponse> rows = await _employeeService.GetSalaryByDepartmentAsync(cancellationToken);
            return Ok(rows);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeResponse>> Create(
            [FromBody] CreateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse created = await _employeeService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> Update(
            int id,
            [FromBody] UpdateEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse updated = await _employeeService.UpdateAsync(id, request, cancellationToken);
            return Ok(updated);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> Patch(
            int id,
            [FromBody] PatchEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            EmployeeResponse patched = await _employeeService.PatchAsync(id, request, cancellationToken);
            return Ok(patched);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}

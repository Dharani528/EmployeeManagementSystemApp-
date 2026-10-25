using EmployeeManagementSystemApp.Week4.Dtos;
using EmployeeManagementSystemApp.Week4.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemApp.Week4.Controllers
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
    }
}

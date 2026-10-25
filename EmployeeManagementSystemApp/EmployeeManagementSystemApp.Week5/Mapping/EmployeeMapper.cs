using EmployeeManagementSystemApp.Week5.Dtos;
using EmployeeManagementSystemApp.Week5.Models;

namespace EmployeeManagementSystemApp.Week5.Mapping
{
    public static class EmployeeMapper
    {
        public static Employee ToEntity(CreateEmployeeRequest request)
        {
            return new Employee
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                Salary = request.Salary,
                DepartmentId = request.DepartmentId,
                JoiningDate = DateTime.UtcNow,
                Skills = request.Skills
                    .Select(skill => new Skill
                    {
                        Name = skill.Name.Trim(),
                        Years = skill.Years
                    })
                    .ToList()
            };
        }

        public static void ApplyUpdate(Employee employee, UpdateEmployeeRequest request)
        {
            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();
            employee.Email = request.Email.Trim();
            employee.Salary = request.Salary;
            employee.DepartmentId = request.DepartmentId;
        }

        public static void ApplyPatch(Employee employee, PatchEmployeeRequest request)
        {
            if (request.FirstName is not null)
                employee.FirstName = request.FirstName.Trim();
            if (request.LastName is not null)
                employee.LastName = request.LastName.Trim();
            if (request.Email is not null)
                employee.Email = request.Email.Trim();
            if (request.Salary.HasValue)
                employee.Salary = request.Salary.Value;
            if (request.DepartmentId.HasValue)
                employee.DepartmentId = request.DepartmentId.Value;
        }
    }
}

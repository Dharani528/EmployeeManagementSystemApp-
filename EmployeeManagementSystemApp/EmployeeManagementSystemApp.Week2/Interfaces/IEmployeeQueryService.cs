using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Interfaces
{
    public interface IEmployeeQueryService
    {
        IReadOnlyList<Employee> FilterByDepartment(string departmentName);
        IReadOnlyList<EmployeeNameSalary> SelectNamesAndSalaries();
        IReadOnlyList<Employee> SortByDepartmentThenSalary();
        IReadOnlyList<DepartmentEmployeeGroup> GroupByDepartment();
        bool AnySalaryGreaterThan(decimal amount);
        bool AllSalariesGreaterThan(decimal amount);
        Employee FindFirstInDepartment(string departmentName);
        Employee? FindFirstOrDefaultInDepartment(string departmentName);
        Employee FindSingleByEmail(string email);
        Employee FindSingleInDepartment(string departmentName);
        int CountEmployees();
        int CountBySalaryAtLeast(decimal amount);
        decimal SumSalaries();
        IReadOnlyList<EmployeeDepartmentMatch> JoinEmployeesWithDepartments();
    }
}

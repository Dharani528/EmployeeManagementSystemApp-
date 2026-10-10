using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Interfaces
{
    public interface IEmployeeService
    {
        Employee AddDeveloper(string firstName, string lastName, string email, string departmentName, decimal salary, string programmingLanguage);
        Employee AddManager(string firstName, string lastName, string email, string departmentName, decimal salary, int teamSize);
        void UpdateEmployee(int employeeId, string firstName, string lastName, string email, string departmentName, decimal salary);
        void DeleteEmployee(int employeeId);
        Employee GetEmployee(int employeeId);
        IReadOnlyList<Employee> GetAllEmployees();
        IReadOnlyList<Employee> SearchByName(string name);
        IReadOnlyList<Department> GetDepartments();
    }
}

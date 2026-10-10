using EmployeeManagementSystemApp.Week2.Models;

namespace EmployeeManagementSystemApp.Week2.Interfaces
{
    public interface IEmployeeRepository
    {
        IReadOnlyList<Employee> GetAll();
        IReadOnlyList<Department> GetDepartments();
        Department? FindDepartmentByName(string departmentName);
        Employee GetById(int employeeId);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int employeeId);
        int GetNextEmployeeId();
        bool EmailExists(string email, int excludingEmployeeId);
    }
}

namespace EmployeeManagementSystemApp.Week2.Models
{
    public class EmployeeNameSalary
    {
        public EmployeeNameSalary(string fullName, string role, decimal salary)
        {
            FullName = fullName;
            Role = role;
            Salary = salary;
        }

        public string FullName { get; }
        public string Role { get; }
        public decimal Salary { get; }
    }

    public class DepartmentEmployeeGroup
    {
        public DepartmentEmployeeGroup(string departmentName, IReadOnlyList<Employee> employees)
        {
            DepartmentName = departmentName;
            Employees = employees;
        }

        public string DepartmentName { get; }
        public IReadOnlyList<Employee> Employees { get; }
    }

    public class EmployeeDepartmentMatch
    {
        public EmployeeDepartmentMatch(string fullName, string role, decimal salary, string departmentName, string location)
        {
            FullName = fullName;
            Role = role;
            Salary = salary;
            DepartmentName = departmentName;
            Location = location;
        }

        public string FullName { get; }
        public string Role { get; }
        public decimal Salary { get; }
        public string DepartmentName { get; }
        public string Location { get; }
    }
}

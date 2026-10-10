namespace EmployeeManagementSystemApp.Week2.Models
{
    public class Department
    {
        public Department(int departmentId, string name, string location)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Department ID must be greater than zero.", nameof(departmentId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Department location is required.", nameof(location));

            DepartmentId = departmentId;
            Name = name.Trim();
            Location = location.Trim();
        }

        public int DepartmentId { get; }
        public string Name { get; }
        public string Location { get; }
    }
}

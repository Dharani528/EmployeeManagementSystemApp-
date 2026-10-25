namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class DepartmentResponse
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}

namespace EmployeeManagementSystemApp.Week6.Dtos
{
    public class PatchEmployeeRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public decimal? Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
}

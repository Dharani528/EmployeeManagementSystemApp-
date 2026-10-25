namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class EmployeeSummaryResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}

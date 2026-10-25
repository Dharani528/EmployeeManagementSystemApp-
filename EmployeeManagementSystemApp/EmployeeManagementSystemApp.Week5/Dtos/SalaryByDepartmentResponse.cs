namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class SalaryByDepartmentResponse
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal AverageSalary { get; set; }
    }
}

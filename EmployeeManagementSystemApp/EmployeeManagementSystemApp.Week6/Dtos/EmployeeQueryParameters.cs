namespace EmployeeManagementSystemApp.Week6.Dtos
{
    public class EmployeeQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public int? DepartmentId { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string SortBy { get; set; } = "employeeId";
        public string SortDir { get; set; } = "asc";
    }
}

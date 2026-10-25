namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class DepartmentDetailResponse
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<EmployeeSummaryResponse> Employees { get; set; } = new List<EmployeeSummaryResponse>();
    }
}

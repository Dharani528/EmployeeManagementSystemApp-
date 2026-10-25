namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class EmployeeResponse
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentSummaryResponse Department { get; set; } = new DepartmentSummaryResponse();
        public List<SkillResponse> Skills { get; set; } = new List<SkillResponse>();
    }
}

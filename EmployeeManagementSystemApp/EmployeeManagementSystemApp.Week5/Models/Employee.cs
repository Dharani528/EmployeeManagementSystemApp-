namespace EmployeeManagementSystemApp.Week5.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}

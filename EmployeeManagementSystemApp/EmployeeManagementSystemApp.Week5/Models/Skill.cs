namespace EmployeeManagementSystemApp.Week5.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Years { get; set; }
        public Employee? Employee { get; set; }
    }
}

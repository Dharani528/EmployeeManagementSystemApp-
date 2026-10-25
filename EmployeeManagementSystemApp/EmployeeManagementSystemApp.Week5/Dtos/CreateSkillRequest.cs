using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class CreateSkillRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 40)]
        public int Years { get; set; }
    }
}

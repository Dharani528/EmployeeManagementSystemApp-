using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemApp.Week4.Dtos
{
    public class PatchEmployeeRequest
    {
        [StringLength(50, MinimumLength = 1)]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Range(0.01, 9999999)]
        public decimal? Salary { get; set; }

        [Range(1, int.MaxValue)]
        public int? DepartmentId { get; set; }
    }
}

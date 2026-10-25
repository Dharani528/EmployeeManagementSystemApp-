using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemApp.Week5.Dtos
{
    public class GiveRaiseRequest
    {
        [Range(0.01, 100)]
        public decimal Percent { get; set; }
    }
}

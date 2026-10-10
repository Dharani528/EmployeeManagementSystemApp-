using EmployeeManagementSystemApp.Week2.Exceptions;

namespace EmployeeManagementSystemApp.Week2.Models
{
    public class Manager : Employee
    {
        private readonly int _teamSize;

        public Manager(
            int employeeId,
            string firstName,
            string lastName,
            string email,
            int departmentId,
            decimal salary,
            int teamSize)
            : base(employeeId, firstName, lastName, email, departmentId, salary)
        {
            if (teamSize < 0)
                throw new InvalidEmployeeException("Team size cannot be negative.");

            _teamSize = teamSize;
        }

        public override string Role => "Manager";

        public int TeamSize => _teamSize;

        public override decimal CalculateBonus()
        {
            return (Salary * 0.15m) + (_teamSize * 1000m);
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"    Team size: {TeamSize}");
        }
    }
}

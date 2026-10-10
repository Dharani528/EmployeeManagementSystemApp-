namespace EmployeeManagementSystemApp.Week2.Models
{
    public class Developer : Employee
    {
        private readonly string _programmingLanguage;

        public Developer(
            int employeeId,
            string firstName,
            string lastName,
            string email,
            int departmentId,
            decimal salary,
            string programmingLanguage)
            : base(employeeId, firstName, lastName, email, departmentId, salary)
        {
            _programmingLanguage = RequireText(programmingLanguage, "Programming language");
        }

        public override string Role => "Developer";

        public string ProgrammingLanguage => _programmingLanguage;

        public override decimal CalculateBonus()
        {
            return Salary * 0.10m;
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"    Language: {ProgrammingLanguage}");
        }
    }
}

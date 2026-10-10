using EmployeeManagementSystemApp.Week2.Exceptions;

namespace EmployeeManagementSystemApp.Week2.Models
{
    public abstract class Employee
    {
        private readonly int _employeeId;
        private readonly DateTime _joiningDate;
        private string _firstName;
        private string _lastName;
        private string _email;
        private int _departmentId;
        private decimal _salary;

        protected Employee(int employeeId, string firstName, string lastName, string email, int departmentId, decimal salary)
        {
            if (employeeId <= 0)
                throw new InvalidEmployeeException("Employee ID must be greater than zero.");

            _employeeId = employeeId;
            _firstName = RequireText(firstName, "First name");
            _lastName = RequireText(lastName, "Last name");
            _email = RequireEmail(email);
            _departmentId = RequireDepartmentId(departmentId);
            _salary = RequireSalary(salary);
            _joiningDate = DateTime.Now;
        }

        public int EmployeeId => _employeeId;

        public DateTime JoiningDate => _joiningDate;

        public abstract string Role { get; }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = RequireText(value, "First name");
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = RequireText(value, "Last name");
        }

        public string Email
        {
            get => _email;
            set => _email = RequireEmail(value);
        }

        public int DepartmentId
        {
            get => _departmentId;
            set => _departmentId = RequireDepartmentId(value);
        }

        public decimal Salary
        {
            get => _salary;
            set => _salary = RequireSalary(value);
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public abstract decimal CalculateBonus();

        public virtual void DisplayDetails()
        {
            Console.WriteLine(
                $"ID {EmployeeId} | {Role} | {GetFullName()} | {Email} | DepartmentId {DepartmentId} | Salary {Salary:C} | Bonus {CalculateBonus():C}");
        }

        protected static string RequireText(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmployeeException($"{fieldName} is required.");

            return value.Trim();
        }

        protected static string RequireEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new InvalidEmployeeException("Email is required and must contain '@'.");

            return email.Trim();
        }

        protected static int RequireDepartmentId(int departmentId)
        {
            if (departmentId <= 0)
                throw new InvalidEmployeeException("Department ID must be greater than zero.");

            return departmentId;
        }

        protected static decimal RequireSalary(decimal salary)
        {
            if (salary <= 0)
                throw new InvalidEmployeeException("Salary must be greater than zero.");

            return salary;
        }
    }
}

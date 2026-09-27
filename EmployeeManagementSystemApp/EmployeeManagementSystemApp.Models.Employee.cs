namespace EmployeeManagementSystemApp.Models
{
    /// <summary>
    /// Employee class represents an employee in the system
    /// This is a model class that holds employee data
    /// </summary>
    public class Employee
    {
        // Properties (characteristics of an employee)
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }

        // Constructor - initializes an Employee object
        public Employee()
        {
            JoiningDate = DateTime.Now;
        }

        // Method to get full name
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        // Method to display employee information
        public override string ToString()
        {
            return $"ID: {EmployeeId} | Name: {GetFullName()} | Email: {Email} | Department: {Department} | Salary: {Salary:C} | Joining Date: {JoiningDate:yyyy-MM-dd}";
        }
    }
}

namespace EmployeeManagementSystemApp.Week4.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message)
            : base(message)
        {
        }
    }
}

namespace EmployeeManagementSystemApp.Week6.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message)
            : base(message)
        {
        }
    }
}

namespace EmployeeManagementSystemApp.Week5.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message)
            : base(message)
        {
        }
    }
}

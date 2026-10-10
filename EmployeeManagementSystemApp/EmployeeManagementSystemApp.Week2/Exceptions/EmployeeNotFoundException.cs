namespace EmployeeManagementSystemApp.Week2.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message)
            : base(message)
        {
        }
    }
}

namespace EmployeeManagementSystemApp.Week6.Exceptions
{
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException(string message)
            : base(message)
        {
        }
    }
}

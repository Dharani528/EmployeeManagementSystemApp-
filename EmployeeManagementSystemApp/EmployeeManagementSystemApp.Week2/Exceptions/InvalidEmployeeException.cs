namespace EmployeeManagementSystemApp.Week2.Exceptions
{
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException(string message)
            : base(message)
        {
        }
    }
}

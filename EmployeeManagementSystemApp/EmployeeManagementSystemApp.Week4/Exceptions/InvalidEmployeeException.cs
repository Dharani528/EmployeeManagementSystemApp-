namespace EmployeeManagementSystemApp.Week4.Exceptions
{
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException(string message)
            : base(message)
        {
        }
    }
}

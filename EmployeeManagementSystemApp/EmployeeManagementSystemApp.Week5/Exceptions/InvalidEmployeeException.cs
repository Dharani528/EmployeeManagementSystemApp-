namespace EmployeeManagementSystemApp.Week5.Exceptions
{
    public class InvalidEmployeeException : Exception
    {
        public InvalidEmployeeException(string message)
            : base(message)
        {
        }
    }
}

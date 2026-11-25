namespace EmployeeManagementSystemApp.Week6.Security
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Hr = "HR";
        public const string Viewer = "Viewer";

        public const string Read = Admin + "," + Hr + "," + Viewer;
        public const string WriteEmployees = Admin;
        public const string GiveRaise = Admin + "," + Hr;
    }
}

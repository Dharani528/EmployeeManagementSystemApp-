using EmployeeManagementSystemApp.Week6.Models;
using EmployeeManagementSystemApp.Week6.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemApp.Week6.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedUsersAsync(AppDbContext dbContext)
        {
            if (await dbContext.Users.AnyAsync())
                return;

            PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

            AppUser admin = new AppUser { UserName = "admin", Role = AppRoles.Admin, DisplayName = "System Admin", IsActive = true };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            AppUser hr = new AppUser { UserName = "hr", Role = AppRoles.Hr, DisplayName = "HR User", IsActive = true };
            hr.PasswordHash = hasher.HashPassword(hr, "Hr@123");

            AppUser viewer = new AppUser { UserName = "viewer", Role = AppRoles.Viewer, DisplayName = "Read Only User", IsActive = true };
            viewer.PasswordHash = hasher.HashPassword(viewer, "Viewer@123");

            dbContext.Users.AddRange(admin, hr, viewer);
            await dbContext.SaveChangesAsync();
        }
    }
}

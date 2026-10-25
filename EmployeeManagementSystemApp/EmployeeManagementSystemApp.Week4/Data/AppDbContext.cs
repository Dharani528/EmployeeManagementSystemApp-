using EmployeeManagementSystemApp.Week4.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemApp.Week4.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.DepartmentId).UseIdentityColumn(4, 1);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Location).IsRequired().HasMaxLength(50);
                entity.HasIndex(d => d.Name).IsUnique();
                entity.HasData(
                    new Department { DepartmentId = 1, Name = "IT", Location = "Hyderabad" },
                    new Department { DepartmentId = 2, Name = "HR", Location = "Bengaluru" },
                    new Department { DepartmentId = 3, Name = "Finance", Location = "Mumbai" });
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId).UseIdentityColumn(1007, 1);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Salary).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasData(
                    new Employee { EmployeeId = 1001, FirstName = "John", LastName = "Smith", Email = "john.smith@company.com", Salary = 75000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 1, 10) },
                    new Employee { EmployeeId = 1002, FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@company.com", Salary = 90000m, DepartmentId = 2, JoiningDate = new DateTime(2024, 2, 12) },
                    new Employee { EmployeeId = 1003, FirstName = "Michael", LastName = "Williams", Email = "michael.williams@company.com", Salary = 80000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 3, 5) },
                    new Employee { EmployeeId = 1004, FirstName = "Priya", LastName = "Sharma", Email = "priya.sharma@company.com", Salary = 120000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 4, 18) },
                    new Employee { EmployeeId = 1005, FirstName = "Amit", LastName = "Patel", Email = "amit.patel@company.com", Salary = 70000m, DepartmentId = 3, JoiningDate = new DateTime(2024, 5, 22) },
                    new Employee { EmployeeId = 1006, FirstName = "David", LastName = "Lee", Email = "david.lee@company.com", Salary = 95000m, DepartmentId = 3, JoiningDate = new DateTime(2024, 6, 8) });
            });
        }
    }
}

using EmployeeManagementSystemApp.Week5.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemApp.Week5.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Skill> Skills => Set<Skill>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(department => department.DepartmentId);
                entity.Property(department => department.DepartmentId).UseIdentityColumn(4, 1);
                entity.Property(department => department.Name).IsRequired().HasMaxLength(50);
                entity.Property(department => department.Location).IsRequired().HasMaxLength(50);
                entity.HasIndex(department => department.Name).IsUnique();
                entity.HasMany(department => department.Employees)
                    .WithOne(employee => employee.Department)
                    .HasForeignKey(employee => employee.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasData(
                    new Department { DepartmentId = 1, Name = "IT", Location = "Hyderabad" },
                    new Department { DepartmentId = 2, Name = "HR", Location = "Bengaluru" },
                    new Department { DepartmentId = 3, Name = "Finance", Location = "Mumbai" });
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(employee => employee.EmployeeId);
                entity.Property(employee => employee.EmployeeId).UseIdentityColumn(1007, 1);
                entity.Property(employee => employee.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(employee => employee.LastName).IsRequired().HasMaxLength(50);
                entity.Property(employee => employee.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(employee => employee.Email).IsUnique();
                entity.Property(employee => employee.Salary).HasColumnType("decimal(10,2)");
                entity.HasMany(employee => employee.Skills)
                    .WithOne(skill => skill.Employee)
                    .HasForeignKey(skill => skill.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                    new Employee { EmployeeId = 1001, FirstName = "John", LastName = "Smith", Email = "john.smith@company.com", Salary = 75000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 1, 10) },
                    new Employee { EmployeeId = 1002, FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@company.com", Salary = 90000m, DepartmentId = 2, JoiningDate = new DateTime(2024, 2, 12) },
                    new Employee { EmployeeId = 1003, FirstName = "Michael", LastName = "Williams", Email = "michael.williams@company.com", Salary = 80000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 3, 5) },
                    new Employee { EmployeeId = 1004, FirstName = "Priya", LastName = "Sharma", Email = "priya.sharma@company.com", Salary = 120000m, DepartmentId = 1, JoiningDate = new DateTime(2024, 4, 18) },
                    new Employee { EmployeeId = 1005, FirstName = "Amit", LastName = "Patel", Email = "amit.patel@company.com", Salary = 70000m, DepartmentId = 3, JoiningDate = new DateTime(2024, 5, 22) },
                    new Employee { EmployeeId = 1006, FirstName = "David", LastName = "Lee", Email = "david.lee@company.com", Salary = 95000m, DepartmentId = 3, JoiningDate = new DateTime(2024, 6, 8) });
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(skill => skill.SkillId);
                entity.Property(skill => skill.SkillId).UseIdentityColumn(8, 1);
                entity.Property(skill => skill.Name).IsRequired().HasMaxLength(50);
                entity.HasData(
                    new Skill { SkillId = 1, EmployeeId = 1001, Name = "C#", Years = 4 },
                    new Skill { SkillId = 2, EmployeeId = 1001, Name = "SQL", Years = 3 },
                    new Skill { SkillId = 3, EmployeeId = 1002, Name = "Recruiting", Years = 5 },
                    new Skill { SkillId = 4, EmployeeId = 1003, Name = "Java", Years = 4 },
                    new Skill { SkillId = 5, EmployeeId = 1004, Name = "C#", Years = 6 },
                    new Skill { SkillId = 6, EmployeeId = 1005, Name = "SQL", Years = 2 },
                    new Skill { SkillId = 7, EmployeeId = 1006, Name = "Excel", Years = 5 });
            });
        }
    }
}

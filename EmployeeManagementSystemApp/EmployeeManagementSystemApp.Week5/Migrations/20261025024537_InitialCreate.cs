using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystemApp.Week5.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "4, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1007, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "8, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Years = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "Hyderabad", "IT" },
                    { 2, "Bengaluru", "HR" },
                    { 3, "Mumbai", "Finance" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "DepartmentId", "Email", "FirstName", "JoiningDate", "LastName", "Salary" },
                values: new object[,]
                {
                    { 1001, 1, "john.smith@company.com", "John", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", 75000m },
                    { 1002, 2, "sarah.johnson@company.com", "Sarah", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johnson", 90000m },
                    { 1003, 1, "michael.williams@company.com", "Michael", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Williams", 80000m },
                    { 1004, 1, "priya.sharma@company.com", "Priya", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sharma", 120000m },
                    { 1005, 3, "amit.patel@company.com", "Amit", new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patel", 70000m },
                    { 1006, 3, "david.lee@company.com", "David", new DateTime(2024, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lee", 95000m }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "SkillId", "EmployeeId", "Name", "Years" },
                values: new object[,]
                {
                    { 1, 1001, "C#", 4 },
                    { 2, 1001, "SQL", 3 },
                    { 3, 1002, "Recruiting", 5 },
                    { 4, 1003, "Java", 4 },
                    { 5, 1004, "C#", 6 },
                    { 6, 1005, "SQL", 2 },
                    { 7, 1006, "Excel", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_EmployeeId",
                table: "Skills",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}

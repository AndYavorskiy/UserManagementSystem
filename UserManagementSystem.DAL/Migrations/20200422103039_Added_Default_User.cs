using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagementSystem.DAL.Migrations
{
    public partial class Added_Default_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "Gender", "IsActive", "LastName", "Password", "PasswordChangeRequired", "Phone", "ProfileImageUrl", "Role" },
                values: new object[] { new Guid("a248e128-1883-4608-9e00-2dd06b90d056"), null, "admin@ums.com", "Admin", 3, true, "Admin", "$UMS$1000$WO5sNyczF1bY00TyYjhvWzaDs7vTf3OcecM2Hyd6UgW4srsr", false, null, null, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a248e128-1883-4608-9e00-2dd06b90d056"));
        }
    }
}

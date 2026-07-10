using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "AddPropertyImages", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ChangePropertyActiveState", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "CreateProperty", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "DeleteProperty", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "DeletePropertyImage", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ManageConfigurations", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ReadMyProperties", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ReadProperties", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ReadProperty", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "SetPrimaryPropertyImage", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "UpdateProperty", "ADMIN" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "AddPropertyImages", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ChangePropertyActiveState", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "CreateProperty", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "DeleteProperty", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "DeletePropertyImage", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ReadMyProperties", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "ReadProperty", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "SetPrimaryPropertyImage", "MANAGER" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionName", "Role" },
                keyValues: new object[] { "UpdateProperty", "MANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionName", "Role" },
                values: new object[,]
                {
                    { "AddPropertyImages", "ADMIN" },
                    { "ChangePropertyActiveState", "ADMIN" },
                    { "CreateProperty", "ADMIN" },
                    { "DeleteProperty", "ADMIN" },
                    { "DeletePropertyImage", "ADMIN" },
                    { "ManageConfigurations", "ADMIN" },
                    { "ReadMyProperties", "ADMIN" },
                    { "ReadProperties", "ADMIN" },
                    { "ReadProperty", "ADMIN" },
                    { "SetPrimaryPropertyImage", "ADMIN" },
                    { "UpdateProperty", "ADMIN" },
                    { "AddPropertyImages", "MANAGER" },
                    { "ChangePropertyActiveState", "MANAGER" },
                    { "CreateProperty", "MANAGER" },
                    { "DeleteProperty", "MANAGER" },
                    { "DeletePropertyImage", "MANAGER" },
                    { "ReadMyProperties", "MANAGER" },
                    { "ReadProperty", "MANAGER" },
                    { "SetPrimaryPropertyImage", "MANAGER" },
                    { "UpdateProperty", "MANAGER" }
                });
        }
    }
}

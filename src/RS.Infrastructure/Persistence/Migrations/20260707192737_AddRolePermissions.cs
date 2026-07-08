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
            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PermissionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.Role, x.PermissionName });
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");
        }
    }
}

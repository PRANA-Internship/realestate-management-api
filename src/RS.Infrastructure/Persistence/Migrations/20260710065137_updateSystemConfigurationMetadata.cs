using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateSystemConfigurationMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "SystemConfigurations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "SystemConfigurations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "SystemConfigurations");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "SystemConfigurations");
        }
    }
}

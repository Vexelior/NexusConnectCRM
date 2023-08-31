using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewHelpInfoPropertiesForViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerWasRecentResponse",
                table: "Help",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmployeeWasRecentResponse",
                table: "Help",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerWasRecentResponse",
                table: "Help");

            migrationBuilder.DropColumn(
                name: "EmployeeWasRecentResponse",
                table: "Help");
        }
    }
}

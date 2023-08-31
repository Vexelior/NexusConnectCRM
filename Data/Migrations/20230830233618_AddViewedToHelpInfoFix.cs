using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddViewedToHelpInfoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerViewed",
                table: "HelpFeedback");

            migrationBuilder.DropColumn(
                name: "EmployeeViewed",
                table: "HelpFeedback");

            migrationBuilder.AddColumn<bool>(
                name: "CustomerViewed",
                table: "Help",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmployeeViewed",
                table: "Help",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerViewed",
                table: "Help");

            migrationBuilder.DropColumn(
                name: "EmployeeViewed",
                table: "Help");

            migrationBuilder.AddColumn<bool>(
                name: "CustomerViewed",
                table: "HelpFeedback",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmployeeViewed",
                table: "HelpFeedback",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

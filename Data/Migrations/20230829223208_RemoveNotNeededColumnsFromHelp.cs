using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotNeededColumnsFromHelp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Help");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseId",
                table: "HelpFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseId",
                table: "HelpFeedback",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ResponseId",
                table: "Help",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

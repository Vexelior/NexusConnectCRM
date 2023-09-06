using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class HelpModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "HelpFeedback");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Help");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "HelpFeedback",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "HelpFeedback");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "HelpFeedback",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Help",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

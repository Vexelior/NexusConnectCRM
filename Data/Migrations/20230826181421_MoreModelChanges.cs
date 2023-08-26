using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoreModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelpResponseInfo_Help_HelpInfoId",
                table: "HelpResponseInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HelpResponseInfo",
                table: "HelpResponseInfo");

            migrationBuilder.RenameTable(
                name: "HelpResponseInfo",
                newName: "HelpFeedback");

            migrationBuilder.RenameIndex(
                name: "IX_HelpResponseInfo_HelpInfoId",
                table: "HelpFeedback",
                newName: "IX_HelpFeedback_HelpInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HelpFeedback",
                table: "HelpFeedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HelpFeedback_Help_HelpInfoId",
                table: "HelpFeedback",
                column: "HelpInfoId",
                principalTable: "Help",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelpFeedback_Help_HelpInfoId",
                table: "HelpFeedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HelpFeedback",
                table: "HelpFeedback");

            migrationBuilder.RenameTable(
                name: "HelpFeedback",
                newName: "HelpResponseInfo");

            migrationBuilder.RenameIndex(
                name: "IX_HelpFeedback_HelpInfoId",
                table: "HelpResponseInfo",
                newName: "IX_HelpResponseInfo_HelpInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HelpResponseInfo",
                table: "HelpResponseInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HelpResponseInfo_Help_HelpInfoId",
                table: "HelpResponseInfo",
                column: "HelpInfoId",
                principalTable: "Help",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHelpImagesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelpImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HelpResponseId = table.Column<int>(type: "int", nullable: false),
                    HelpId = table.Column<int>(type: "int", nullable: false),
                    IsHelpResponse = table.Column<bool>(type: "bit", nullable: false),
                    IsHelp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpImages_HelpFeedback_HelpResponseId",
                        column: x => x.HelpResponseId,
                        principalTable: "HelpFeedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HelpImages_Help_HelpId",
                        column: x => x.HelpId,
                        principalTable: "Help",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpImages_HelpId",
                table: "HelpImages",
                column: "HelpId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpImages_HelpResponseId",
                table: "HelpImages",
                column: "HelpResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpImages");
        }
    }
}

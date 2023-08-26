using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class HelpResponseChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelpResponseInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEmployee = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HelpInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpResponseInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpResponseInfo_Help_HelpInfoId",
                        column: x => x.HelpInfoId,
                        principalTable: "Help",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpResponseInfo_HelpInfoId",
                table: "HelpResponseInfo",
                column: "HelpInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpResponseInfo");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusConnectCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserRemoveContactProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "IsHelped",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "NeedsContact",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "NeedsHelp",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsHelped",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NeedsContact",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NeedsHelp",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsHelped",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NeedsContact",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "NeedsHelp",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "Prospects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHelped",
                table: "Prospects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsContact",
                table: "Prospects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsHelp",
                table: "Prospects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHelped",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsContact",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsHelp",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "Customers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHelped",
                table: "Customers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsContact",
                table: "Customers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsHelp",
                table: "Customers",
                type: "bit",
                nullable: true);
        }
    }
}

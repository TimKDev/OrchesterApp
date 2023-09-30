using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RegisterKey",
                table: "Orchestermitglieder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserLastLogin",
                table: "Orchestermitglieder",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLastLogin",
                table: "Orchestermitglieder");

            migrationBuilder.AlterColumn<string>(
                name: "RegisterKey",
                table: "Orchestermitglieder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoreAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectedUserId",
                table: "Orchestermitglieder",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisterKey",
                table: "Orchestermitglieder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterKeyExpirationDate",
                table: "Orchestermitglieder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UserFirstConnected",
                table: "Orchestermitglieder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Orchestermitglieder_ConnectedUserId",
                table: "Orchestermitglieder",
                column: "ConnectedUserId",
                unique: true,
                filter: "[ConnectedUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder",
                column: "ConnectedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropIndex(
                name: "IX_Orchestermitglieder_ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "RegisterKey",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "RegisterKeyExpirationDate",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "UserFirstConnected",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");
        }
    }
}

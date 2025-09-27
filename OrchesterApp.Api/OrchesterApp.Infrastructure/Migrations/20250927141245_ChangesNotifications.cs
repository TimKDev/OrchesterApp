using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserNotifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserNotifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

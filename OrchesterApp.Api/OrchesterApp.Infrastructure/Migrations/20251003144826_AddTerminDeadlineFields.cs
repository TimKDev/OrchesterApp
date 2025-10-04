using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTerminDeadlineFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ErsteWarnungVorFrist",
                table: "Termine",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Frist",
                table: "Termine",
                type: "interval",
                nullable: true);

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
                name: "ErsteWarnungVorFrist",
                table: "Termine");

            migrationBuilder.DropColumn(
                name: "Frist",
                table: "Termine");

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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMemberSinceToOrchesterMitglied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.AddColumn<DateTime>(
                name: "MemberSince",
                table: "Orchestermitglieder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberSinceInYears",
                table: "Orchestermitglieder",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropColumn(
                name: "MemberSince",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "MemberSinceInYears",
                table: "Orchestermitglieder");

            migrationBuilder.AddForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

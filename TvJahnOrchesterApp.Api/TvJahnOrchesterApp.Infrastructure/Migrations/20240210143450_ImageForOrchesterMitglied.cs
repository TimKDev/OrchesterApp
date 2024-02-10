using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageForOrchesterMitglied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Orchestermitglieder",
                type: "nvarchar(max)",
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
                name: "Image",
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

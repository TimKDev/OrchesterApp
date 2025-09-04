using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedMitgliedSinceYears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropColumn(
                name: "MemberSinceInYears",
                table: "Orchestermitglieder");

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

            migrationBuilder.AddColumn<int>(
                name: "MemberSinceInYears",
                table: "Orchestermitglieder",
                type: "integer",
                nullable: true);

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

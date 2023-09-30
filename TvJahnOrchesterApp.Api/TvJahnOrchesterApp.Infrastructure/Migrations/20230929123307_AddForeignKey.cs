using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_OrchesterMitgliedsId",
                table: "TerminRückmeldungen",
                column: "OrchesterMitgliedsId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_RückmeldungDurchAnderesOrchestermitglied",
                table: "TerminRückmeldungen",
                column: "RückmeldungDurchAnderesOrchestermitglied");

            migrationBuilder.AddForeignKey(
                name: "FK_TerminRückmeldungen_Orchestermitglieder_OrchesterMitgliedsId",
                table: "TerminRückmeldungen",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TerminRückmeldungen_Orchestermitglieder_RückmeldungDurchAnderesOrchestermitglied",
                table: "TerminRückmeldungen",
                column: "RückmeldungDurchAnderesOrchestermitglied",
                principalTable: "Orchestermitglieder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TerminRückmeldungen_Orchestermitglieder_OrchesterMitgliedsId",
                table: "TerminRückmeldungen");

            migrationBuilder.DropForeignKey(
                name: "FK_TerminRückmeldungen_Orchestermitglieder_RückmeldungDurchAnderesOrchestermitglied",
                table: "TerminRückmeldungen");

            migrationBuilder.DropIndex(
                name: "IX_TerminRückmeldungen_OrchesterMitgliedsId",
                table: "TerminRückmeldungen");

            migrationBuilder.DropIndex(
                name: "IX_TerminRückmeldungen_RückmeldungDurchAnderesOrchestermitglied",
                table: "TerminRückmeldungen");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTerminImageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropColumn(name: "Image",
                table: "Termine");

            migrationBuilder.AddColumn<byte[]>(
                            name: "Image",
                            table: "Termine",
                            type: "varbinary(max)",
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

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Termine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

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

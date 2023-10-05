using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TerminDropdowns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropTable(
                name: "EinsatzplanNoten");

            migrationBuilder.DropTable(
                name: "EinsatzplanUniform");

            migrationBuilder.DropTable(
                name: "RückmeldungInstruments");

            migrationBuilder.DropTable(
                name: "RückmeldungNotenstimmen");

            migrationBuilder.AlterColumn<int>(
                name: "TerminArt",
                table: "Termine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TerminStatus",
                table: "Termine",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Positions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notenstimme",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MitgliedsStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Instrumente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ArtInstrument",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "Noten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rückmeldungsarten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rückmeldungsarten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminArten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminArten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungInstrumentMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstrumentId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TerminRückmeldungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminRückmeldungInstrumentMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungInstrumentMapping_Instrumente_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instrumente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungInstrumentMapping_TerminRückmeldungen_TerminRückmeldungsId_TerminId",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungNotenstimmeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotenstimmenId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TerminRückmeldungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminRückmeldungNotenstimmeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungNotenstimmeMapping_Notenstimme_NotenstimmenId",
                        column: x => x.NotenstimmenId,
                        principalTable: "Notenstimme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungNotenstimmeMapping_TerminRückmeldungen_TerminRückmeldungsId_TerminId",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uniform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uniform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EinsatzplanNotenMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotenId = table.Column<int>(type: "int", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinsatzplanNotenMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EinsatzplanNotenMapping_Einsatzpläne_EinsatzplanId_TerminId",
                        columns: x => new { x.EinsatzplanId, x.TerminId },
                        principalTable: "Einsatzpläne",
                        principalColumns: new[] { "EinsatzplanId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EinsatzplanNotenMapping_Noten_NotenId",
                        column: x => x.NotenId,
                        principalTable: "Noten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EinsatzplanUniformMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniformId = table.Column<int>(type: "int", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinsatzplanUniformMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EinsatzplanUniformMapping_Einsatzpläne_EinsatzplanId_TerminId",
                        columns: x => new { x.EinsatzplanId, x.TerminId },
                        principalTable: "Einsatzpläne",
                        principalColumns: new[] { "EinsatzplanId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EinsatzplanUniformMapping_Uniform_UniformId",
                        column: x => x.UniformId,
                        principalTable: "Uniform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Noten",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Schwarzes Marschbuch" },
                    { 2, "Blaues Marschbuch" },
                    { 3, "Rotes Marschbuch" },
                    { 4, "Konzertmappe" },
                    { 5, "Weihnachtsnoten" },
                    { 6, "St. Martin Noten" },
                    { 7, "Karnevalsnoten" }
                });

            migrationBuilder.InsertData(
                table: "Rückmeldungsarten",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Nicht Zurückgemeldet" },
                    { 2, "Abgesagt" },
                    { 3, "Zugesagt" }
                });

            migrationBuilder.InsertData(
                table: "TerminArten",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Auftritt" },
                    { 2, "Probe" },
                    { 3, "Konzert" },
                    { 4, "Freizeit" },
                    { 5, "Reise" }
                });

            migrationBuilder.InsertData(
                table: "TerminStatus",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Angefragt" },
                    { 2, "Zugesagt" },
                    { 3, "Abgesagt" }
                });

            migrationBuilder.InsertData(
                table: "Uniform",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Blaues Hemd" },
                    { 2, "Weiße Hose" },
                    { 3, "Jacket" },
                    { 4, "Winter Jacke" },
                    { 5, "Zivil" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Termine_TerminArt",
                table: "Termine",
                column: "TerminArt");

            migrationBuilder.CreateIndex(
                name: "IX_Termine_TerminStatus",
                table: "Termine",
                column: "TerminStatus");

            migrationBuilder.CreateIndex(
                name: "IX_EinsatzplanNotenMapping_EinsatzplanId_TerminId",
                table: "EinsatzplanNotenMapping",
                columns: new[] { "EinsatzplanId", "TerminId" });

            migrationBuilder.CreateIndex(
                name: "IX_EinsatzplanNotenMapping_NotenId",
                table: "EinsatzplanNotenMapping",
                column: "NotenId");

            migrationBuilder.CreateIndex(
                name: "IX_EinsatzplanUniformMapping_EinsatzplanId_TerminId",
                table: "EinsatzplanUniformMapping",
                columns: new[] { "EinsatzplanId", "TerminId" });

            migrationBuilder.CreateIndex(
                name: "IX_EinsatzplanUniformMapping_UniformId",
                table: "EinsatzplanUniformMapping",
                column: "UniformId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungInstrumentMapping_InstrumentId",
                table: "TerminRückmeldungInstrumentMapping",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungInstrumentMapping_TerminRückmeldungsId_TerminId",
                table: "TerminRückmeldungInstrumentMapping",
                columns: new[] { "TerminRückmeldungsId", "TerminId" });

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungNotenstimmeMapping_NotenstimmenId",
                table: "TerminRückmeldungNotenstimmeMapping",
                column: "NotenstimmenId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungNotenstimmeMapping_TerminRückmeldungsId_TerminId",
                table: "TerminRückmeldungNotenstimmeMapping",
                columns: new[] { "TerminRückmeldungsId", "TerminId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Termine_TerminArten_TerminArt",
                table: "Termine",
                column: "TerminArt",
                principalTable: "TerminArten",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Termine_TerminStatus_TerminStatus",
                table: "Termine",
                column: "TerminStatus",
                principalTable: "TerminStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Termine_TerminArten_TerminArt",
                table: "Termine");

            migrationBuilder.DropForeignKey(
                name: "FK_Termine_TerminStatus_TerminStatus",
                table: "Termine");

            migrationBuilder.DropTable(
                name: "EinsatzplanNotenMapping");

            migrationBuilder.DropTable(
                name: "EinsatzplanUniformMapping");

            migrationBuilder.DropTable(
                name: "Rückmeldungsarten");

            migrationBuilder.DropTable(
                name: "TerminArten");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungInstrumentMapping");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungNotenstimmeMapping");

            migrationBuilder.DropTable(
                name: "TerminStatus");

            migrationBuilder.DropTable(
                name: "Noten");

            migrationBuilder.DropTable(
                name: "Uniform");

            migrationBuilder.DropIndex(
                name: "IX_Termine_TerminArt",
                table: "Termine");

            migrationBuilder.DropIndex(
                name: "IX_Termine_TerminStatus",
                table: "Termine");

            migrationBuilder.DropColumn(
                name: "TerminStatus",
                table: "Termine");

            migrationBuilder.AlterColumn<int>(
                name: "TerminArt",
                table: "Termine",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Positions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notenstimme",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MitgliedsStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Instrumente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ArtInstrument",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "EinsatzplanNoten",
                columns: table => new
                {
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotenEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinsatzplanNoten", x => new { x.EinsatzplanId, x.TerminId, x.Id });
                    table.ForeignKey(
                        name: "FK_EinsatzplanNoten_Einsatzpläne_EinsatzplanId_TerminId",
                        columns: x => new { x.EinsatzplanId, x.TerminId },
                        principalTable: "Einsatzpläne",
                        principalColumns: new[] { "EinsatzplanId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EinsatzplanUniform",
                columns: table => new
                {
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniformEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinsatzplanUniform", x => new { x.EinsatzplanId, x.TerminId, x.Id });
                    table.ForeignKey(
                        name: "FK_EinsatzplanUniform_Einsatzpläne_EinsatzplanId_TerminId",
                        columns: x => new { x.EinsatzplanId, x.TerminId },
                        principalTable: "Einsatzpläne",
                        principalColumns: new[] { "EinsatzplanId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RückmeldungInstruments",
                columns: table => new
                {
                    TerminRückmeldungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RückmeldungInstruments", x => new { x.TerminRückmeldungsId, x.TerminId, x.Id });
                    table.ForeignKey(
                        name: "FK_RückmeldungInstruments_TerminRückmeldungen_TerminRückmeldungsId_TerminId",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RückmeldungNotenstimmen",
                columns: table => new
                {
                    TerminRückmeldungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RückmeldungNotenstimmen", x => new { x.TerminRückmeldungsId, x.TerminId, x.Id });
                    table.ForeignKey(
                        name: "FK_RückmeldungNotenstimmen_TerminRückmeldungen_TerminRückmeldungsId_TerminId",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

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

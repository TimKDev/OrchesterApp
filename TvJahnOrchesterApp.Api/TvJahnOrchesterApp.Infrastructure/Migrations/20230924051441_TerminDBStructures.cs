using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TerminDBStructures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultNotenStimme",
                table: "Orchestermitglieder",
                newName: "DefaultNotenStimme_Stimme");

            migrationBuilder.CreateTable(
                name: "Termine",
                columns: table => new
                {
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TerminArt = table.Column<int>(type: "int", nullable: false),
                    AbstimmungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termine", x => x.TerminId);
                });

            migrationBuilder.CreateTable(
                name: "Einsatzpläne",
                columns: table => new
                {
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartZeit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndZeit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Treffpunkt_Straße = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treffpunkt_Hausnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treffpunkt_Postleitzahl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treffpunkt_Stadt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Treffpunkt_Zusatz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treffpunkt_Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Treffpunkt_Longitide = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeitereInformationen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Einsatzpläne", x => new { x.EinsatzplanId, x.TerminId });
                    table.ForeignKey(
                        name: "FK_Einsatzpläne_Termine_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termine",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungen",
                columns: table => new
                {
                    TerminRückmeldungsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zugesagt = table.Column<int>(type: "int", nullable: false),
                    KommentarZusage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetzteRückmeldung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RückmeldungDurchAnderesOrchestermitglied = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IstAnwesend = table.Column<bool>(type: "bit", nullable: false),
                    KommentarAnwesenheit = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminRückmeldungen", x => new { x.TerminRückmeldungsId, x.TerminId });
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungen_Termine_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termine",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Zeitblöcke",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TerminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Startzeit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Endzeit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Beschreibung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Straße = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Hausnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Postleitzahl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Stadt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Zusatz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Adresse_Longitide = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zeitblöcke", x => new { x.Id, x.EinsatzplanId, x.TerminId });
                    table.ForeignKey(
                        name: "FK_Zeitblöcke_Einsatzpläne_EinsatzplanId_TerminId",
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtInstrument = table.Column<int>(type: "int", nullable: false)
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
                    Stimme = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Einsatzpläne_TerminId",
                table: "Einsatzpläne",
                column: "TerminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_TerminId",
                table: "TerminRückmeldungen",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Zeitblöcke_EinsatzplanId_TerminId",
                table: "Zeitblöcke",
                columns: new[] { "EinsatzplanId", "TerminId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EinsatzplanNoten");

            migrationBuilder.DropTable(
                name: "EinsatzplanUniform");

            migrationBuilder.DropTable(
                name: "RückmeldungInstruments");

            migrationBuilder.DropTable(
                name: "RückmeldungNotenstimmen");

            migrationBuilder.DropTable(
                name: "Zeitblöcke");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungen");

            migrationBuilder.DropTable(
                name: "Einsatzpläne");

            migrationBuilder.DropTable(
                name: "Termine");

            migrationBuilder.RenameColumn(
                name: "DefaultNotenStimme_Stimme",
                table: "Orchestermitglieder",
                newName: "DefaultNotenStimme");
        }
    }
}

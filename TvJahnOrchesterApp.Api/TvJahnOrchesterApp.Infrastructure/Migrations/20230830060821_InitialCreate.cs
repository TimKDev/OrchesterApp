using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orchestermitglieder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nachname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Straße = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Hausnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Postleitzahl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Stadt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Zusatz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Adresse_Longitide = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Geburtstag = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Handynummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultInstrument_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultInstrument_ArtInstrument = table.Column<int>(type: "int", nullable: false),
                    DefaultNotenStimme = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orchestermitglieder", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orchestermitglieder");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TvJahnOrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropdownsOrchestermitglied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RückmeldungInstruments");

            migrationBuilder.DropColumn(
                name: "DefaultInstrument_ArtInstrument",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "DefaultInstrument_Name",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "DefaultNotenStimme_Stimme",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "OrchesterMitgliedsStatus_MitgliedsStatusEnum",
                table: "Orchestermitglieder");

            migrationBuilder.RenameColumn(
                name: "Stimme",
                table: "RückmeldungNotenstimmen",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ArtInstrument",
                table: "RückmeldungInstruments",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orchestermitglieder",
                newName: "OrchesterMitgliedsId");

            migrationBuilder.AddColumn<int>(
                name: "DefaultInstrument",
                table: "Orchestermitglieder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultNotenStimme",
                table: "Orchestermitglieder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrchesterMitgliedsStatus",
                table: "Orchestermitglieder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArtInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MitgliedsStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MitgliedsStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notenstimme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notenstimme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrumente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArtInstrumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrumente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instrumente_ArtInstrument_ArtInstrumentId",
                        column: x => x.ArtInstrumentId,
                        principalTable: "ArtInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrchestermitgliedPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrchestermitgliedPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMitgliedsId",
                        column: x => x.OrchesterMitgliedsId,
                        principalTable: "Orchestermitglieder",
                        principalColumn: "OrchesterMitgliedsId");
                    table.ForeignKey(
                        name: "FK_OrchestermitgliedPositions_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ArtInstrument",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Holz" },
                    { 2, "Blech" },
                    { 3, "Schlagwerk" },
                    { 4, "Dirigent" }
                });

            migrationBuilder.InsertData(
                table: "MitgliedsStatus",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "aktiv" },
                    { 2, "passiv" },
                    { 3, "ausgetreten" }
                });

            migrationBuilder.InsertData(
                table: "Notenstimme",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Alt Saxophon 1" },
                    { 2, "Alt Saxophon 2" },
                    { 3, "Sopran Saxophon" },
                    { 4, "Trompete 1" },
                    { 5, "Trompete 2" },
                    { 6, "Trompete 3" },
                    { 7, "Schlagzeug" }
                });

            migrationBuilder.InsertData(
                table: "Instrumente",
                columns: new[] { "Id", "ArtInstrumentId", "Value" },
                values: new object[,]
                {
                    { 1, 1, "Saxophon" },
                    { 2, 2, "Trompete" },
                    { 3, 3, "Trommel" },
                    { 4, 4, "Dirigent" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orchestermitglieder_DefaultInstrument",
                table: "Orchestermitglieder",
                column: "DefaultInstrument");

            migrationBuilder.CreateIndex(
                name: "IX_Orchestermitglieder_DefaultNotenStimme",
                table: "Orchestermitglieder",
                column: "DefaultNotenStimme");

            migrationBuilder.CreateIndex(
                name: "IX_Orchestermitglieder_OrchesterMitgliedsStatus",
                table: "Orchestermitglieder",
                column: "OrchesterMitgliedsStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Instrumente_ArtInstrumentId",
                table: "Instrumente",
                column: "ArtInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrchestermitgliedPositions_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrchestermitgliedPositions_PositionId",
                table: "OrchestermitgliedPositions",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder",
                column: "ConnectedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_Instrumente_DefaultInstrument",
                table: "Orchestermitglieder",
                column: "DefaultInstrument",
                principalTable: "Instrumente",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_MitgliedsStatus_OrchesterMitgliedsStatus",
                table: "Orchestermitglieder",
                column: "OrchesterMitgliedsStatus",
                principalTable: "MitgliedsStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_Notenstimme_DefaultNotenStimme",
                table: "Orchestermitglieder",
                column: "DefaultNotenStimme",
                principalTable: "Notenstimme",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_Instrumente_DefaultInstrument",
                table: "Orchestermitglieder");

            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_MitgliedsStatus_OrchesterMitgliedsStatus",
                table: "Orchestermitglieder");

            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_Notenstimme_DefaultNotenStimme",
                table: "Orchestermitglieder");

            migrationBuilder.DropTable(
                name: "Instrumente");

            migrationBuilder.DropTable(
                name: "MitgliedsStatus");

            migrationBuilder.DropTable(
                name: "Notenstimme");

            migrationBuilder.DropTable(
                name: "OrchestermitgliedPositions");

            migrationBuilder.DropTable(
                name: "ArtInstrument");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Orchestermitglieder_DefaultInstrument",
                table: "Orchestermitglieder");

            migrationBuilder.DropIndex(
                name: "IX_Orchestermitglieder_DefaultNotenStimme",
                table: "Orchestermitglieder");

            migrationBuilder.DropIndex(
                name: "IX_Orchestermitglieder_OrchesterMitgliedsStatus",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "DefaultInstrument",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "DefaultNotenStimme",
                table: "Orchestermitglieder");

            migrationBuilder.DropColumn(
                name: "OrchesterMitgliedsStatus",
                table: "Orchestermitglieder");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "RückmeldungNotenstimmen",
                newName: "Stimme");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "RückmeldungInstruments",
                newName: "ArtInstrument");

            migrationBuilder.RenameColumn(
                name: "OrchesterMitgliedsId",
                table: "Orchestermitglieder",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RückmeldungInstruments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DefaultInstrument_ArtInstrument",
                table: "Orchestermitglieder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultInstrument_Name",
                table: "Orchestermitglieder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DefaultNotenStimme_Stimme",
                table: "Orchestermitglieder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrchesterMitgliedsStatus_MitgliedsStatusEnum",
                table: "Orchestermitglieder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    OrchesterMitgliedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => new { x.OrchesterMitgliedId, x.Id });
                    table.ForeignKey(
                        name: "FK_Position_Orchestermitglieder_OrchesterMitgliedId",
                        column: x => x.OrchesterMitgliedId,
                        principalTable: "Orchestermitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder",
                column: "ConnectedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

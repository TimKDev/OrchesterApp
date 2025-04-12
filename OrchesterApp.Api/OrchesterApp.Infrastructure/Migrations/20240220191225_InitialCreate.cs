using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrchesterApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MitgliedsStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MitgliedsStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Noten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notenstimme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notenstimme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rückmeldungsarten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rückmeldungsarten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminArten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminArten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uniform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uniform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrumente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ArtInstrumentId = table.Column<int>(type: "integer", nullable: false)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Termine",
                columns: table => new
                {
                    TerminId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true),
                    TerminArt = table.Column<int>(type: "integer", nullable: true),
                    TerminStatus = table.Column<int>(type: "integer", nullable: true),
                    AbstimmungsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termine", x => x.TerminId);
                    table.ForeignKey(
                        name: "FK_Termine_TerminArten_TerminArt",
                        column: x => x.TerminArt,
                        principalTable: "TerminArten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Termine_TerminStatus_TerminStatus",
                        column: x => x.TerminStatus,
                        principalTable: "TerminStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Einsatzpläne",
                columns: table => new
                {
                    EinsatzplanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartZeit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndZeit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Treffpunkt_Straße = table.Column<string>(type: "text", nullable: true),
                    Treffpunkt_Hausnummer = table.Column<string>(type: "text", nullable: true),
                    Treffpunkt_Postleitzahl = table.Column<string>(type: "text", nullable: true),
                    Treffpunkt_Stadt = table.Column<string>(type: "text", nullable: true),
                    Treffpunkt_Zusatz = table.Column<string>(type: "text", nullable: true),
                    Treffpunkt_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Treffpunkt_Longitide = table.Column<decimal>(type: "numeric", nullable: true),
                    WeitereInformationen = table.Column<string>(type: "text", nullable: true)
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
                name: "EinsatzplanNotenMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotenId = table.Column<int>(type: "integer", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uuid", nullable: true),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniformId = table.Column<int>(type: "integer", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uuid", nullable: true),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EinsatzplanUniformMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EinsatzplanUniformMapping_Einsatzpläne_EinsatzplanId_Termin~",
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

            migrationBuilder.CreateTable(
                name: "Zeitblöcke",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EinsatzplanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: false),
                    Startzeit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Endzeit = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Beschreibung = table.Column<string>(type: "text", nullable: false),
                    Adresse_Straße = table.Column<string>(type: "text", nullable: true),
                    Adresse_Hausnummer = table.Column<string>(type: "text", nullable: true),
                    Adresse_Postleitzahl = table.Column<string>(type: "text", nullable: true),
                    Adresse_Stadt = table.Column<string>(type: "text", nullable: true),
                    Adresse_Zusatz = table.Column<string>(type: "text", nullable: true),
                    Adresse_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Adresse_Longitide = table.Column<decimal>(type: "numeric", nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orchestermitglieder",
                columns: table => new
                {
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Vorname = table.Column<string>(type: "text", nullable: false),
                    Nachname = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true),
                    Adresse_Straße = table.Column<string>(type: "text", nullable: true),
                    Adresse_Hausnummer = table.Column<string>(type: "text", nullable: true),
                    Adresse_Postleitzahl = table.Column<string>(type: "text", nullable: true),
                    Adresse_Stadt = table.Column<string>(type: "text", nullable: true),
                    Adresse_Zusatz = table.Column<string>(type: "text", nullable: true),
                    Adresse_Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Adresse_Longitide = table.Column<decimal>(type: "numeric", nullable: true),
                    Geburtstag = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Telefonnummer = table.Column<string>(type: "text", nullable: true),
                    Handynummer = table.Column<string>(type: "text", nullable: true),
                    DefaultInstrument = table.Column<int>(type: "integer", nullable: true),
                    DefaultNotenStimme = table.Column<int>(type: "integer", nullable: true),
                    RegisterKey = table.Column<string>(type: "text", nullable: false),
                    RegisterKeyExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConnectedUserId = table.Column<string>(type: "text", nullable: true),
                    UserFirstConnected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserLastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MemberSince = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MemberSinceInYears = table.Column<int>(type: "integer", nullable: true),
                    OrchesterMitgliedsStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orchestermitglieder", x => x.OrchesterMitgliedsId);
                    table.ForeignKey(
                        name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                        column: x => x.ConnectedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orchestermitglieder_Instrumente_DefaultInstrument",
                        column: x => x.DefaultInstrument,
                        principalTable: "Instrumente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orchestermitglieder_MitgliedsStatus_OrchesterMitgliedsStatus",
                        column: x => x.OrchesterMitgliedsStatus,
                        principalTable: "MitgliedsStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orchestermitglieder_Notenstimme_DefaultNotenStimme",
                        column: x => x.DefaultNotenStimme,
                        principalTable: "Notenstimme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrchestermitgliedPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<int>(type: "integer", nullable: false),
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrchestermitgliedPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrchestermitgliedPositions_Orchestermitglieder_OrchesterMit~",
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

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungen",
                columns: table => new
                {
                    TerminRückmeldungsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrchesterMitgliedsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Zugesagt = table.Column<int>(type: "integer", nullable: false),
                    KommentarZusage = table.Column<string>(type: "text", nullable: true),
                    LetzteRückmeldung = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RückmeldungDurchAnderesOrchestermitglied = table.Column<Guid>(type: "uuid", nullable: true),
                    IstAnwesend = table.Column<bool>(type: "boolean", nullable: false),
                    KommentarAnwesenheit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminRückmeldungen", x => new { x.TerminRückmeldungsId, x.TerminId });
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungen_Orchestermitglieder_OrchesterMitgliedsId",
                        column: x => x.OrchesterMitgliedsId,
                        principalTable: "Orchestermitglieder",
                        principalColumn: "OrchesterMitgliedsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungen_Orchestermitglieder_RückmeldungDurchAnd~",
                        column: x => x.RückmeldungDurchAnderesOrchestermitglied,
                        principalTable: "Orchestermitglieder",
                        principalColumn: "OrchesterMitgliedsId");
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungen_Termine_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termine",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungInstrumentMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InstrumentId = table.Column<int>(type: "integer", nullable: false),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: true),
                    TerminRückmeldungsId = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_TerminRückmeldungInstrumentMapping_TerminRückmeldungen_Term~",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminRückmeldungNotenstimmeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotenstimmenId = table.Column<int>(type: "integer", nullable: false),
                    TerminId = table.Column<Guid>(type: "uuid", nullable: true),
                    TerminRückmeldungsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminRückmeldungNotenstimmeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungNotenstimmeMapping_Notenstimme_Notenstimme~",
                        column: x => x.NotenstimmenId,
                        principalTable: "Notenstimme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminRückmeldungNotenstimmeMapping_TerminRückmeldungen_Ter~",
                        columns: x => new { x.TerminRückmeldungsId, x.TerminId },
                        principalTable: "TerminRückmeldungen",
                        principalColumns: new[] { "TerminRückmeldungsId", "TerminId" },
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
                table: "Positions",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "Dirigent" },
                    { 2, "Obmann" },
                    { 3, "Kassierer" },
                    { 4, "Notenwart" },
                    { 5, "Zeugwart" },
                    { 6, "Thekenteam" }
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrchesterMitgliedsId",
                table: "AspNetUsers",
                column: "OrchesterMitgliedsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Einsatzpläne_TerminId",
                table: "Einsatzpläne",
                column: "TerminId",
                unique: true);

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
                name: "IX_Instrumente_ArtInstrumentId",
                table: "Instrumente",
                column: "ArtInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orchestermitglieder_ConnectedUserId",
                table: "Orchestermitglieder",
                column: "ConnectedUserId",
                unique: true);

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
                name: "IX_OrchestermitgliedPositions_OrchesterMitgliedsId",
                table: "OrchestermitgliedPositions",
                column: "OrchesterMitgliedsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrchestermitgliedPositions_PositionId",
                table: "OrchestermitgliedPositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Termine_TerminArt",
                table: "Termine",
                column: "TerminArt");

            migrationBuilder.CreateIndex(
                name: "IX_Termine_TerminStatus",
                table: "Termine",
                column: "TerminStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_OrchesterMitgliedsId",
                table: "TerminRückmeldungen",
                column: "OrchesterMitgliedsId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_RückmeldungDurchAnderesOrchestermitglied",
                table: "TerminRückmeldungen",
                column: "RückmeldungDurchAnderesOrchestermitglied");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungen_TerminId",
                table: "TerminRückmeldungen",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungInstrumentMapping_InstrumentId",
                table: "TerminRückmeldungInstrumentMapping",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungInstrumentMapping_TerminRückmeldungsId_Ter~",
                table: "TerminRückmeldungInstrumentMapping",
                columns: new[] { "TerminRückmeldungsId", "TerminId" });

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungNotenstimmeMapping_NotenstimmenId",
                table: "TerminRückmeldungNotenstimmeMapping",
                column: "NotenstimmenId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminRückmeldungNotenstimmeMapping_TerminRückmeldungsId_Te~",
                table: "TerminRückmeldungNotenstimmeMapping",
                columns: new[] { "TerminRückmeldungsId", "TerminId" });

            migrationBuilder.CreateIndex(
                name: "IX_Zeitblöcke_EinsatzplanId_TerminId",
                table: "Zeitblöcke",
                columns: new[] { "EinsatzplanId", "TerminId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Orchestermitglieder_OrchesterMitgliedsId",
                table: "AspNetUsers",
                column: "OrchesterMitgliedsId",
                principalTable: "Orchestermitglieder",
                principalColumn: "OrchesterMitgliedsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orchestermitglieder_AspNetUsers_ConnectedUserId",
                table: "Orchestermitglieder");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EinsatzplanNotenMapping");

            migrationBuilder.DropTable(
                name: "EinsatzplanUniformMapping");

            migrationBuilder.DropTable(
                name: "OrchestermitgliedPositions");

            migrationBuilder.DropTable(
                name: "Rückmeldungsarten");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungInstrumentMapping");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungNotenstimmeMapping");

            migrationBuilder.DropTable(
                name: "Zeitblöcke");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Noten");

            migrationBuilder.DropTable(
                name: "Uniform");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "TerminRückmeldungen");

            migrationBuilder.DropTable(
                name: "Einsatzpläne");

            migrationBuilder.DropTable(
                name: "Termine");

            migrationBuilder.DropTable(
                name: "TerminArten");

            migrationBuilder.DropTable(
                name: "TerminStatus");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Orchestermitglieder");

            migrationBuilder.DropTable(
                name: "Instrumente");

            migrationBuilder.DropTable(
                name: "MitgliedsStatus");

            migrationBuilder.DropTable(
                name: "Notenstimme");

            migrationBuilder.DropTable(
                name: "ArtInstrument");
        }
    }
}

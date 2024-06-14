using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RulesEngine8.Migrations
{
    /// <inheritdoc />
    public partial class add_initial_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceID = table.Column<string>(type: "text", nullable: true),
                    AssetID = table.Column<string>(type: "text", nullable: true),
                    DigitalInputsJson = table.Column<string>(type: "text", nullable: false),
                    Config = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isAlarm = table.Column<bool>(type: "boolean", nullable: false),
                    alarmId = table.Column<string>(type: "text", nullable: true),
                    assetId = table.Column<string>(type: "text", nullable: true),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    emailSent = table.Column<bool>(type: "boolean", nullable: false),
                    emailContent = table.Column<string>(type: "text", nullable: true),
                    emailRecipient = table.Column<string>(type: "text", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleChains",
                columns: table => new
                {
                    RuleChainId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    NodesJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleChains", x => x.RuleChainId);
                });

            migrationBuilder.CreateTable(
                name: "DigitalInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alarmId = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    shortDescription = table.Column<string>(type: "text", nullable: true),
                    longDescription = table.Column<string>(type: "text", nullable: true),
                    isTPST = table.Column<bool>(type: "boolean", nullable: false),
                    sendEmail = table.Column<bool>(type: "boolean", nullable: false),
                    Invert = table.Column<bool>(type: "boolean", nullable: false),
                    IsAlarm = table.Column<bool>(type: "boolean", nullable: false),
                    InstallationType = table.Column<string>(type: "text", nullable: true),
                    InstallationKey = table.Column<string>(type: "text", nullable: true),
                    SensorKey = table.Column<string>(type: "text", nullable: true),
                    SensorType = table.Column<string>(type: "text", nullable: true),
                    SendOnChange = table.Column<bool>(type: "boolean", nullable: false),
                    Send = table.Column<bool>(type: "boolean", nullable: false),
                    Error = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMsg = table.Column<string>(type: "text", nullable: true),
                    LastTime = table.Column<string>(type: "text", nullable: true),
                    topIsActive = table.Column<bool>(type: "boolean", nullable: false),
                    topIndex = table.Column<int>(type: "integer", nullable: false),
                    DIIndex = table.Column<int>(type: "integer", nullable: false),
                    NumChannels = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<string>(type: "text", nullable: true),
                    topColor = table.Column<long>(type: "bigint", nullable: true),
                    ConfigItemID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalInputs_ConfigItems_ConfigItemID",
                        column: x => x.ConfigItemID,
                        principalTable: "ConfigItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleNodes",
                columns: table => new
                {
                    RuleNodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NodeUUID = table.Column<string>(type: "text", nullable: false),
                    NodeType = table.Column<string>(type: "text", nullable: false),
                    ConfigurationJson = table.Column<string>(type: "text", nullable: false),
                    RuleChainID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleNodes", x => x.RuleNodeId);
                    table.ForeignKey(
                        name: "FK_RuleNodes_RuleChains_RuleChainID",
                        column: x => x.RuleChainID,
                        principalTable: "RuleChains",
                        principalColumn: "RuleChainId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DigitalInputs_ConfigItemID",
                table: "DigitalInputs",
                column: "ConfigItemID");

            migrationBuilder.CreateIndex(
                name: "IX_RuleNodes_RuleChainID",
                table: "RuleNodes",
                column: "RuleChainID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DigitalInputs");

            migrationBuilder.DropTable(
                name: "HistoryTables");

            migrationBuilder.DropTable(
                name: "RuleNodes");

            migrationBuilder.DropTable(
                name: "ConfigItems");

            migrationBuilder.DropTable(
                name: "RuleChains");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RulesEngine8.Migrations
{
    /// <inheritdoc />
    public partial class Add_DB_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DigitalInputsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isAlarm = table.Column<bool>(type: "bit", nullable: false),
                    alarmId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emailSent = table.Column<bool>(type: "bit", nullable: false),
                    emailContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emailRecipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    timestampLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAlarm = table.Column<bool>(type: "bit", nullable: false),
                    alarmStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alarmId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigitalInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    alarmId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    shortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    longDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isTPST = table.Column<bool>(type: "bit", nullable: false),
                    sendEmail = table.Column<bool>(type: "bit", nullable: false),
                    Invert = table.Column<bool>(type: "bit", nullable: false),
                    IsAlarm = table.Column<bool>(type: "bit", nullable: false),
                    InstallationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallationKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendOnChange = table.Column<bool>(type: "bit", nullable: false),
                    Send = table.Column<bool>(type: "bit", nullable: false),
                    Error = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMsg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    topIsActive = table.Column<bool>(type: "bit", nullable: false),
                    topIndex = table.Column<int>(type: "int", nullable: false),
                    DIIndex = table.Column<int>(type: "int", nullable: false),
                    NumChannels = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    topColor = table.Column<long>(type: "bigint", nullable: true),
                    ConfigItemID = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DigitalInputs_ConfigItemID",
                table: "DigitalInputs",
                column: "ConfigItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DigitalInputs");

            migrationBuilder.DropTable(
                name: "HistoryTables");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "ConfigItems");
        }
    }
}

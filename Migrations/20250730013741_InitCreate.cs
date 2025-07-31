using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewECO5.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterSettings",
                columns: table => new
                {
                    SerialNr = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeterID = table.Column<byte>(type: "smallint", nullable: false),
                    IsActived = table.Column<bool>(type: "boolean", nullable: false),
                    ModbusMode = table.Column<int>(type: "integer", nullable: true),
                    Protocol_Id = table.Column<short>(type: "smallint", nullable: false),
                    MetersGroupSetting_Id = table.Column<short>(type: "smallint", nullable: false),
                    ProtocolStr = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterSettings", x => x.SerialNr);
                });

            migrationBuilder.CreateTable(
                name: "MetersGroupSettings",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConnectionStr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsTCPSetting = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetersGroupSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Brand = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WiringMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Loop = table.Column<short>(type: "smallint", nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreateUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdateUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerMeterSettings",
                columns: table => new
                {
                    SerialNr = table.Column<int>(type: "integer", nullable: false),
                    KVA_Diff = table.Column<float>(type: "real", nullable: false),
                    KW_Check_limit = table.Column<float>(type: "real", nullable: false),
                    BufferToKeep = table.Column<int>(type: "integer", nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DiagramNr = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsLowVoltage = table.Column<bool>(type: "boolean", nullable: false),
                    AutoReset = table.Column<bool>(type: "boolean", nullable: false),
                    Dmd_Interval = table.Column<float>(type: "real", nullable: false),
                    Dmd_Capacity = table.Column<float>(type: "real", nullable: false),
                    Dmd_CapacityHalf = table.Column<float>(type: "real", nullable: false),
                    Dmd_CapacitySatHalf = table.Column<float>(type: "real", nullable: false),
                    Dmd_CapacityOff = table.Column<float>(type: "real", nullable: false),
                    Dmd_UpperLimit = table.Column<float>(type: "real", nullable: false),
                    Dmd_LowerLimit = table.Column<float>(type: "real", nullable: false),
                    Dmd_Mode = table.Column<int>(type: "integer", nullable: false),
                    Alarm_I_UpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_I_LowerLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_V_Type = table.Column<int>(type: "integer", nullable: false),
                    Alarm_V_UpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_V_LowerLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_HzUpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_HzLowerLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_PfUpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_PfLowerLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_KWh_UpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_kW_UpperLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_kW_LowerLimit = table.Column<float>(type: "real", nullable: false),
                    Alarm_kW_IdelValue = table.Column<float>(type: "real", nullable: false),
                    Alarm_notPowerOff_kW = table.Column<float>(type: "real", nullable: false),
                    Alarm_notPowerOffAlarmStartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Alarm_notPowerOffAlarmEndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Area = table.Column<float>(type: "real", nullable: true),
                    Persons = table.Column<int>(type: "integer", nullable: true),
                    CustomName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CustomValue = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerMeterSettings", x => x.SerialNr);
                    table.ForeignKey(
                        name: "FK_PowerMeterSettings_MeterSettings_SerialNr",
                        column: x => x.SerialNr,
                        principalTable: "MeterSettings",
                        principalColumn: "SerialNr",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterSettings_SerialNr",
                table: "MeterSettings",
                column: "SerialNr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetersGroupSettings");

            migrationBuilder.DropTable(
                name: "PowerMeterSettings");

            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.DropTable(
                name: "MeterSettings");
        }
    }
}

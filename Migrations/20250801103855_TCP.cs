using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewECO5.Migrations
{
    /// <inheritdoc />
    public partial class TCP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "MeterSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Comm_UnitSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "Comm_UnitSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeterName = table.Column<string>(type: "text", nullable: false),
                    Power = table.Column<float>(type: "real", nullable: false),
                    Voltage = table.Column<float>(type: "real", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterSettings_MetersGroupSetting_Id",
                table: "MeterSettings",
                column: "MetersGroupSetting_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterSettings_Comm_UnitSettings_MetersGroupSetting_Id",
                table: "MeterSettings",
                column: "MetersGroupSetting_Id",
                principalTable: "Comm_UnitSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterSettings_Comm_UnitSettings_MetersGroupSetting_Id",
                table: "MeterSettings");

            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropIndex(
                name: "IX_MeterSettings_MetersGroupSetting_Id",
                table: "MeterSettings");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "MeterSettings");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "Comm_UnitSettings");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Comm_UnitSettings");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirQuality.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    Ozone = table.Column<double>(type: "float", nullable: false),
                    PM_1_0 = table.Column<double>(type: "float", nullable: false),
                    PM_2_5 = table.Column<double>(type: "float", nullable: false),
                    PM_10_0 = table.Column<double>(type: "float", nullable: false),
                    TVOC = table.Column<double>(type: "float", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApiID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorsData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorsData");
        }
    }
}

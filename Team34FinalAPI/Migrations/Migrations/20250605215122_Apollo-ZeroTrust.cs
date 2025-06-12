using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class ApolloZeroTrust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 7,
                column: "Transmission",
                value: "Automatic");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 8,
                column: "HasCanopy",
                value: true);

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 10,
                column: "Transmission",
                value: "Automatic");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 13,
                column: "CabinType",
                value: "Double");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 7,
                column: "Transmission",
                value: "Manual");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 8,
                column: "HasCanopy",
                value: false);

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 10,
                column: "Transmission",
                value: "Manual");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 13,
                column: "CabinType",
                value: "Single");
        }
    }
}

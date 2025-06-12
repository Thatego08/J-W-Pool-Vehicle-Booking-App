using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class Apollo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 19);

            migrationBuilder.AddColumn<string>(
                name: "CabinType",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriveType",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasCanopy",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTowBar",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Transmission",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 1,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Hatch", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 2,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Extra", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 3,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Extra", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 4,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x4", false, true, "Automatic" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 5,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Single", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 6,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x4", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 7,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Extra", "4x2", false, true, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 8,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x4", false, true, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 9,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x4", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 10,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x4", true, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 11,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Single", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 12,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Single", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 13,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Single", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 14,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Double", "4x2", true, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 15,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Extra", "4x2", false, false, "Manual" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 16,
                columns: new[] { "CabinType", "Description", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "SUV", "SUV 1.8 XR CVT ", "4x2", false, false, "Automatic" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 20,
                columns: new[] { "CabinType", "DriveType", "HasCanopy", "HasTowBar", "Transmission" },
                values: new object[] { "Single", "4x2", false, false, "Manual" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CabinType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriveType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "HasCanopy",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "HasTowBar",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "Vehicles");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 16,
                column: "Description",
                value: "1.8 XR CVT ");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleID", "ColourID", "DateAcquired", "Description", "EngineNo", "FuelTypeID", "InsuranceCoverID", "LicenseExpiryDate", "Name", "RegistrationNumber", "StatusID", "VIN", "VehicleMakeID", "VehicleModelID" },
                values: new object[,]
                {
                    { 17, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC 2.8 GD-6RB, 4x2", "1GD0374210", 2, 1, new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S1", " HH 91 VZ GP ", 1, "AHTGA3DD900968871", 11, 2 },
                    { 18, 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2 ", "", 2, 1, new DateTime(2028, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S2", " FV 69 WP GP ", 1, "", 11, 2 },
                    { 19, 2, new DateTime(2005, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2", "2GD0329959", 2, 1, new DateTime(2027, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S3", "HC 85 FY GP", 1, "AHTJB8DBX04575081", 11, 2 }
                });
        }
    }
}

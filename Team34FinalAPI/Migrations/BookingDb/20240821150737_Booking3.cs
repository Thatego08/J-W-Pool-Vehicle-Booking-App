using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class Booking3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "FuelAmount",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "HasAccidents",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "Battery",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "BrakeFluidLevel",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "ClutchFluidLevel",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "RadiatorWaterLevel",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "SpareWheelCondition",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "VBeltCondition",
                table: "RefuelVehicle");

            migrationBuilder.DropColumn(
                name: "WindowWasherFluidLevel",
                table: "RefuelVehicle");

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "RefuelVehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle");

            migrationBuilder.AddColumn<decimal>(
                name: "FuelAmount",
                table: "Trip",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccidents",
                table: "Trip",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "RefuelVehicle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Battery",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrakeFluidLevel",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClutchFluidLevel",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RadiatorWaterLevel",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpareWheelCondition",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VBeltCondition",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WindowWasherFluidLevel",
                table: "RefuelVehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class Checklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Project_ProjectId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Status_StatusId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Vehicles_VehicleId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Booking_BookingID",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

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

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_VehicleId",
                table: "Bookings",
                newName: "IX_Bookings_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_StatusId",
                table: "Bookings",
                newName: "IX_Bookings_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ProjectId",
                table: "Bookings",
                newName: "IX_Bookings_ProjectId");

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "RefuelVehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingID");

            migrationBuilder.CreateTable(
                name: "PreChecklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningKms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OilLeaks = table.Column<bool>(type: "bit", nullable: false),
                    FuelLevel = table.Column<bool>(type: "bit", nullable: false),
                    Mirrors = table.Column<bool>(type: "bit", nullable: false),
                    SunVisor = table.Column<bool>(type: "bit", nullable: false),
                    SeatBelts = table.Column<bool>(type: "bit", nullable: false),
                    HeadLights = table.Column<bool>(type: "bit", nullable: false),
                    Indicators = table.Column<bool>(type: "bit", nullable: false),
                    ParkLights = table.Column<bool>(type: "bit", nullable: false),
                    BrakeLights = table.Column<bool>(type: "bit", nullable: false),
                    StrobeLight = table.Column<bool>(type: "bit", nullable: false),
                    ReverseLight = table.Column<bool>(type: "bit", nullable: false),
                    ReverseHooter = table.Column<bool>(type: "bit", nullable: false),
                    Horn = table.Column<bool>(type: "bit", nullable: false),
                    WindscreenWiper = table.Column<bool>(type: "bit", nullable: false),
                    TyreCondition = table.Column<bool>(type: "bit", nullable: false),
                    SparewheelPresent = table.Column<bool>(type: "bit", nullable: false),
                    JackAndWheelSpannerPresent = table.Column<bool>(type: "bit", nullable: false),
                    Brakes = table.Column<bool>(type: "bit", nullable: false),
                    Handbrake = table.Column<bool>(type: "bit", nullable: false),
                    JwMarketingMagnets = table.Column<bool>(type: "bit", nullable: false),
                    CheckedByJwSecurity = table.Column<bool>(type: "bit", nullable: false),
                    LicenseDiskValid = table.Column<bool>(type: "bit", nullable: false),
                    OilLeaksComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelLevelComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MirrorsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SunVisorComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatBeltsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadLightsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndicatorsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkLightsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrakeLightsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrobeLightComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReverseLightComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReverseHooterComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HornComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WindscreenWiperComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyreConditionComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SparewheelPresentComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JackAndWheelSpannerPresentComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrakesComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandbrakeComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwMarketingMagnetsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckedByJwSecurityComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseDiskValidComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreChecklists_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreChecklists_BookingId",
                table: "PreChecklists",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Project_ProjectId",
                table: "Bookings",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Status_StatusId",
                table: "Bookings",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Vehicles_VehicleId",
                table: "Bookings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Bookings_BookingID",
                table: "Trip",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "BookingID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Project_ProjectId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Status_StatusId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Vehicles_VehicleId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Bookings_BookingID",
                table: "Trip");

            migrationBuilder.DropTable(
                name: "PreChecklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_VehicleId",
                table: "Booking",
                newName: "IX_Booking_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_StatusId",
                table: "Booking",
                newName: "IX_Booking_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ProjectId",
                table: "Booking",
                newName: "IX_Booking_ProjectId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Project_ProjectId",
                table: "Booking",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Status_StatusId",
                table: "Booking",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Vehicles_VehicleId",
                table: "Booking",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefuelVehicle_Trip_TripId",
                table: "RefuelVehicle",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Booking_BookingID",
                table: "Trip",
                column: "BookingID",
                principalTable: "Booking",
                principalColumn: "BookingID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

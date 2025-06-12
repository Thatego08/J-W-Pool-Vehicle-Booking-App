using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.TripDb
{
    /// <inheritdoc />
    public partial class Awaken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostChecks",
                columns: table => new
                {
                    PostCheckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosingKms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    SpareWheelPresent = table.Column<bool>(type: "bit", nullable: false),
                    JackAndWheelSpannerPresent = table.Column<bool>(type: "bit", nullable: false),
                    Brakes = table.Column<bool>(type: "bit", nullable: false),
                    Handbrake = table.Column<bool>(type: "bit", nullable: false),
                    JWMarketingMagnets = table.Column<bool>(type: "bit", nullable: false),
                    CheckedByJWSecurity = table.Column<bool>(type: "bit", nullable: false),
                    LicenseDiskValid = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostChecks", x => x.PostCheckId);
                });

          

            migrationBuilder.CreateTable(
                name: "TripMedia",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostCheckId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripMedia", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_TripMedia_PostChecks_PostCheckId",
                        column: x => x.PostCheckId,
                        principalTable: "PostChecks",
                        principalColumn: "PostCheckId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    SpareWheelPresent = table.Column<bool>(type: "bit", nullable: false),
                    JackAndWheelSpannerPresent = table.Column<bool>(type: "bit", nullable: false),
                    Brakes = table.Column<bool>(type: "bit", nullable: false),
                    Handbrake = table.Column<bool>(type: "bit", nullable: false),
                    JWMarketingMagnets = table.Column<bool>(type: "bit", nullable: false),
                    CheckedByJWSecurity = table.Column<bool>(type: "bit", nullable: false),
                    LicenseDiskValid = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreChecklists_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TravelEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreChecklistId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_PreChecklists_PreChecklistId",
                        column: x => x.PreChecklistId,
                        principalTable: "PreChecklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefuelVehicles",
                columns: table => new
                {
                    RefuelVehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    OilLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyrePressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyreCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FuelCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefuelVehicles", x => x.RefuelVehicleId);
                    table.ForeignKey(
                        name: "FK_RefuelVehicles_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId");
                });

           
            migrationBuilder.CreateIndex(
                name: "IX_PreChecklists_BookingID",
                table: "PreChecklists",
                column: "BookingID");


            migrationBuilder.CreateIndex(
                name: "IX_RefuelVehicles_TripId",
                table: "RefuelVehicles",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripMedia_PostCheckId",
                table: "TripMedia",
                column: "PostCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BookingID",
                table: "Trips",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PreChecklistId",
                table: "Trips",
                column: "PreChecklistId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "RefuelVehicles");

            migrationBuilder.DropTable(
                name: "TripMedia");

            migrationBuilder.DropTable(
                name: "RateType");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "PostChecks");

            migrationBuilder.DropTable(
                name: "PreChecklists");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}

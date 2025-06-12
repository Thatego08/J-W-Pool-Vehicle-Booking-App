using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class Prime35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_RateType_RateTypeID",
                table: "Rate");

            migrationBuilder.DropTable(
                name: "RateType");

            migrationBuilder.DropIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "RateTypeID",
                table: "Rate");

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                computedColumnSql: "CASE \r\n        WHEN Description LIKE '%DC%' THEN 'Double Cab'\r\n        WHEN Description LIKE '%SC%' THEN 'Single Cab'\r\n        WHEN Description LIKE '%Extra Cab%' THEN 'Extra Cab'\r\n        ELSE 'Other'\r\n    END");

            migrationBuilder.CreateTable(
                name: "PostCheck",
                columns: table => new
                {
                    PostCheckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_PostCheck", x => x.PostCheckId);
                    table.ForeignKey(
                        name: "FK_PostCheck_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

          

            migrationBuilder.CreateIndex(
                name: "IX_PostCheck_TripId",
                table: "PostCheck",
                column: "TripId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripMedia");

            migrationBuilder.DropTable(
                name: "PostCheck");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "RateTypeID",
                table: "Rate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RateType",
                columns: table => new
                {
                    RateTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateType", x => x.RateTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate",
                column: "RateTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_RateType_RateTypeID",
                table: "Rate",
                column: "RateTypeID",
                principalTable: "RateType",
                principalColumn: "RateTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

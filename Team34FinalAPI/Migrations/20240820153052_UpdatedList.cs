using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreChecklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
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
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreChecklists_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreChecklists_TripId",
                table: "PreChecklists",
                column: "TripId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreChecklists");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class ServiceUpdate20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Service_VehicleID",
                table: "Service");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Service_VehicleID",
                table: "Service",
                column: "VehicleID");
        }
    }
}

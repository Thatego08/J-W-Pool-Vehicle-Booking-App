using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.BookingDb
{
    public partial class AddRateTypeSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create RateTypes table (new)
            migrationBuilder.CreateTable(
                name: "RateTypes",
                columns: table => new
                {
                    RateTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateTypes", x => x.RateTypeID);
                });

            // Add RateTypeID column to existing Rate table (nullable)
            migrationBuilder.AddColumn<int>(
                name: "RateTypeID",
                table: "Rate",
                type: "int",
                nullable: true);

            // Create index for performance
            migrationBuilder.CreateIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate",
                column: "RateTypeID");

            // Add foreign key from Rate to RateTypes
            migrationBuilder.AddForeignKey(
                name: "FK_Rate_RateTypes_RateTypeID",
                table: "Rate",
                column: "RateTypeID",
                principalTable: "RateTypes",
                principalColumn: "RateTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_RateTypes_RateTypeID",
                table: "Rate");

            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate");

            // Drop column
            migrationBuilder.DropColumn(
                name: "RateTypeID",
                table: "Rate");

            // Drop RateTypes table
            migrationBuilder.DropTable(
                name: "RateTypes");
        }
    }
}
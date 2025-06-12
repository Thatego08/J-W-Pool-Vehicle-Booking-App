using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class NewPolo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Compliance",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Protection",
                table: "Vehicles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 1,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 2,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Sasol Sasolburg", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 3,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Private Use", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 4,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Sasol Secunda", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 5,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 6,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Venetia", "ROPS" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 7,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 8,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Sasol Secunda", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 9,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Sishen", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 10,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 11,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Medupi", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 12,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 13,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 14,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "Sasol Secunda", "None" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 15,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 16,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 20,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compliance",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Protection",
                table: "Vehicles");
        }
    }
}

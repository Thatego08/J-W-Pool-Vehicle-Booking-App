using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Team34FinalAPI.Migrations.VehicleDb
{
    /// <inheritdoc />
    public partial class NewPoloPrime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Protection",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 1,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 2,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 3,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 4,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 5,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 7,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 8,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 9,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 10,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 11,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 12,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 13,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 14,
                column: "Protection",
                value: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 20,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Protection",
                table: "Vehicles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

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
                column: "Protection",
                value: "None");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 3,
                column: "Protection",
                value: "None");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 4,
                column: "Protection",
                value: "None");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 5,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });

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
                column: "Protection",
                value: "None");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 9,
                column: "Protection",
                value: "None");

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
                column: "Protection",
                value: "None");

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
                column: "Protection",
                value: "None");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleID",
                keyValue: 20,
                columns: new[] { "Compliance", "Protection" },
                values: new object[] { "None", "None" });
        }
    }
}

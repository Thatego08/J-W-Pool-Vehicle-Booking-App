using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Team34FinalAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                });




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







            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectNumber = table.Column<int>(type: "int", nullable: false),
                    JobNo = table.Column<int>(type: "int", nullable: false),
                    TaskCode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityCode = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                });



            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    RateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateTypeID = table.Column<int>(type: "int", nullable: false),
                    RateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    ApplicableTimePeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.RateID);
                    table.ForeignKey(
                        name: "FK_Rate_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rate_RateType_RateTypeID",
                        column: x => x.RateTypeID,
                        principalTable: "RateType",
                        principalColumn: "RateTypeID",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });









            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectID", "ActivityCode", "Description", "JobNo", "ProjectNumber", "StatusId", "TaskCode" },
                values: new object[,]
                {
                    { 1, 3000, "None", 1000, 120, 6, 2000 },
                    { 2, 3001, "Mpumalanga Star Project", 1001, 121, 6, 2001 },
                    { 3, 3002, "Gauteng Star Project", 1002, 122, 1, 2002 },
                    { 4, 3003, "Limpopo Star Project", 1003, 123, 2, 2003 },
                    { 5, 3004, "Western Cape Star Project", 1004, 124, 7, 2004 },
                    { 6, 3005, "Free State Star Project", 1005, 125, 7, 2005 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingID", "EndDate", "Event", "ProjectId", "ReminderSent", "StartDate", "StatusId", "Type", "UserName", "VehicleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event1", 1, false, new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Event", "user1", 1 },
                    { 2, new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event2", 2, true, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Project", "user2", 2 },
                    { 3, new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mpumalanga Project", 3, true, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Project", "SabeMa", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ProjectId",
                table: "Bookings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StatusId",
                table: "Bookings",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");



            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_ProjectID",
                table: "Rate",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate",
                column: "RateTypeID");










        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Events");



            migrationBuilder.DropTable(
                name: "Rate");



            migrationBuilder.DropTable(
                name: "Projects");




        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Team34FinalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Awaken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colour", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCover",
                columns: table => new
                {
                    InsuranceCoverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceCoverName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCover", x => x.InsuranceCoverId);
                });

            migrationBuilder.CreateTable(
                name: "PostDocumentation",
                columns: table => new
                {
                    LicenseDisks = table.Column<bool>(type: "bit", nullable: false),
                    CheckedBySecurity = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
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
                name: "Service",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceID);
                    table.UniqueConstraint("AK_Service_VehicleID", x => x.VehicleID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMake",
                columns: table => new
                {
                    VehicleMakeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMake", x => x.VehicleMakeID);
                });

            migrationBuilder.CreateTable(
                name: "Project",
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
                    table.PrimaryKey("PK_Project", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Project_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleModel",
                columns: table => new
                {
                    VehicleModelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleMakeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModel", x => x.VehicleModelID);
                    table.ForeignKey(
                        name: "FK_VehicleModel_VehicleMake_VehicleMakeID",
                        column: x => x.VehicleMakeID,
                        principalTable: "VehicleMake",
                        principalColumn: "VehicleMakeID",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Rate_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
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
                name: "Vehicles",
                columns: table => new
                {
                    VehicleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAcquired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceCoverID = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColourID = table.Column<int>(type: "int", nullable: false),
                    FuelTypeID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    VehicleMakeID = table.Column<int>(type: "int", nullable: false),
                    VehicleModelID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleID);
                    table.ForeignKey(
                        name: "FK_Vehicles_Colour_ColourID",
                        column: x => x.ColourID,
                        principalTable: "Colour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_FuelTypes_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_InsuranceCover_InsuranceCoverID",
                        column: x => x.InsuranceCoverID,
                        principalTable: "InsuranceCover",
                        principalColumn: "InsuranceCoverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleMake_VehicleMakeID",
                        column: x => x.VehicleMakeID,
                        principalTable: "VehicleMake",
                        principalColumn: "VehicleMakeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleModel_VehicleModelID",
                        column: x => x.VehicleModelID,
                        principalTable: "VehicleModel",
                        principalColumn: "VehicleModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
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
                    table.PrimaryKey("PK_Booking", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Booking_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_Booking_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseDisks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseDisks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseDisks_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostChecklist",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnVehicle = table.Column<bool>(type: "bit", nullable: false),
                    OpeningKms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingKms = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostChecklist", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostChecklist_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleChecklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OpeningKms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExteriorChecksMirrors = table.Column<bool>(name: "ExteriorChecks_Mirrors", type: "bit", nullable: false),
                    ExteriorChecksOilWaterLeaks = table.Column<bool>(name: "ExteriorChecks_OilWaterLeaks", type: "bit", nullable: false),
                    ExteriorChecksHeadLights = table.Column<bool>(name: "ExteriorChecks_HeadLights", type: "bit", nullable: false),
                    ExteriorChecksParkLights = table.Column<bool>(name: "ExteriorChecks_ParkLights", type: "bit", nullable: false),
                    ExteriorChecksBrakeLights = table.Column<bool>(name: "ExteriorChecks_BrakeLights", type: "bit", nullable: false),
                    ExteriorChecksStrobeLights = table.Column<bool>(name: "ExteriorChecks_StrobeLights", type: "bit", nullable: false),
                    ExteriorChecksReverseLight = table.Column<bool>(name: "ExteriorChecks_ReverseLight", type: "bit", nullable: false),
                    ExteriorChecksTyreCondition = table.Column<bool>(name: "ExteriorChecks_TyreCondition", type: "bit", nullable: false),
                    ExteriorChecksSpareWheelPresent = table.Column<bool>(name: "ExteriorChecks_SpareWheelPresent", type: "bit", nullable: false),
                    ExteriorChecksDamageToInteriorBodywork = table.Column<bool>(name: "ExteriorChecks_DamageToInteriorBodywork", type: "bit", nullable: false),
                    ExteriorChecksMarketingMagnets = table.Column<bool>(name: "ExteriorChecks_MarketingMagnets", type: "bit", nullable: false),
                    InteriorChecksHorn = table.Column<bool>(name: "InteriorChecks_Horn", type: "bit", nullable: false),
                    InteriorChecksSeatbelt = table.Column<bool>(name: "InteriorChecks_Seatbelt", type: "bit", nullable: false),
                    InteriorChecksSunVisor = table.Column<bool>(name: "InteriorChecks_SunVisor", type: "bit", nullable: false),
                    InteriorChecksSunblock = table.Column<bool>(name: "InteriorChecks_Sunblock", type: "bit", nullable: false),
                    InteriorChecksWindscreen = table.Column<bool>(name: "InteriorChecks_Windscreen", type: "bit", nullable: false),
                    UnderTheHoodChecksFuelLevel = table.Column<bool>(name: "UnderTheHoodChecks_FuelLevel", type: "bit", nullable: false),
                    FunctionalTestsIndicator = table.Column<bool>(name: "FunctionalTests_Indicator", type: "bit", nullable: false),
                    FunctionalTestsReverseHooter = table.Column<bool>(name: "FunctionalTests_ReverseHooter", type: "bit", nullable: false),
                    FunctionalTestsBrakes = table.Column<bool>(name: "FunctionalTests_Brakes", type: "bit", nullable: false),
                    FunctionalTestsHandbrake = table.Column<bool>(name: "FunctionalTests_Handbrake", type: "bit", nullable: false),
                    SafetyEquipmentFireExtinguisher = table.Column<bool>(name: "SafetyEquipment_FireExtinguisher", type: "bit", nullable: false),
                    SafetyEquipmentInspectionValid = table.Column<bool>(name: "SafetyEquipment_InspectionValid", type: "bit", nullable: false),
                    SafetyEquipmentTriangleInPlace3x = table.Column<bool>(name: "SafetyEquipment_TriangleInPlace3x", type: "bit", nullable: false),
                    SafetyEquipmentJackWheelPresent = table.Column<bool>(name: "SafetyEquipment_JackWheelPresent", type: "bit", nullable: false),
                    DocumentationLicenseDisks = table.Column<bool>(name: "Documentation_LicenseDisks", type: "bit", nullable: false),
                    DocumentationCheckedBySecurity = table.Column<bool>(name: "Documentation_CheckedBySecurity", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleChecklists", x => x.Id);
                    table.UniqueConstraint("AK_VehicleChecklists_UserName", x => x.UserName);
                    table.ForeignKey(
                        name: "FK_VehicleChecklists_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreChecklist",
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
                    table.PrimaryKey("PK_PreChecklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreChecklist_Booking_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
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
                    PreChecklistId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trip_Booking_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trip_PreChecklist_PreChecklistId",
                        column: x => x.PreChecklistId,
                        principalTable: "PreChecklist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trip_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID");
                });

            migrationBuilder.CreateTable(
                name: "RefuelVehicle",
                columns: table => new
                {
                    RefuelVehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    OilLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyrePressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyreCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefuelVehicle", x => x.RefuelVehicleId);
                    table.ForeignKey(
                        name: "FK_RefuelVehicle_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Colour",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, " Black" },
                    { 2, " White " },
                    { 3, " Blue" },
                    { 4, "Green" },
                    { 5, " Grey" },
                    { 6, "Red" },
                    { 7, " Silver" }
                });

            migrationBuilder.InsertData(
                table: "FuelTypes",
                columns: new[] { "Id", "FuelName" },
                values: new object[,]
                {
                    { 1, "Petrol" },
                    { 2, "Diesel" }
                });

            migrationBuilder.InsertData(
                table: "InsuranceCover",
                columns: new[] { "InsuranceCoverId", "InsuranceCoverName" },
                values: new object[,]
                {
                    { 1, " By Seun" },
                    { 2, "Comprehensive " },
                    { 3, " Third-party " }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "ServiceID", "AdminEmail", "AdminName", "Description", "ServiceDate", "VehicleID" },
                values: new object[] { 1, "u22492161@tuks.co.za", "Busi", " Doors not closing", new DateTime(2024, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "DateChanged", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Available" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Booked" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "In For Service" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancelled" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complete" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "In-Progress" }
                });

            migrationBuilder.InsertData(
                table: "VehicleMake",
                columns: new[] { "VehicleMakeID", "Name" },
                values: new object[,]
                {
                    { 1, "Audi" },
                    { 2, "Brandt BRV" },
                    { 3, "Citroen" },
                    { 4, "Ford" },
                    { 5, "Haval" },
                    { 6, "Honda" },
                    { 7, "Hyundai" },
                    { 8, "Kia" },
                    { 9, "Isuzu" },
                    { 10, "Nissan" },
                    { 11, "Toyota" }
                });

            migrationBuilder.InsertData(
                table: "VehicleModel",
                columns: new[] { "VehicleModelID", "VehicleMakeID", "VehicleModelName" },
                values: new object[,]
                {
                    { 1, 11, " Corolla Cross" },
                    { 2, 11, " Hilux " },
                    { 3, 9, " Izuzu" },
                    { 4, 1, " A3" },
                    { 5, 1, " A4 " },
                    { 6, 7, " Grand i10" },
                    { 7, 7, " Venue " },
                    { 8, 7, " TUCSON" },
                    { 9, 4, " Ranger" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleID", "ColourID", "DateAcquired", "Description", "EngineNo", "FuelTypeID", "InsuranceCoverID", "LicenseExpiryDate", "Name", "RegistrationNumber", "StatusID", "VIN", "VehicleMakeID", "VehicleModelID" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "i20", "G4LFPV302509", 1, 1, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hyundai 1", "LN 68 YM GP", 1, "MALBG512LPM254886", 7, 7 },
                    { 2, 2, new DateTime(2019, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Extra Cab 2.4 GD-6 RB SRX 6MT ", "2GDC598667", 2, 1, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 1", "JF 72 WJ GP", 1, "AHTJB8DC404730166", 11, 2 },
                    { 3, 2, new DateTime(2020, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Extra Cab 2.4 GD-6 RB SRX 6MT", "2GDC765766", 2, 1, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 2", "JT 99 LF GP", 1, "AHTJB3DCX04490835", 11, 2 },
                    { 4, 2, new DateTime(2024, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC HILUX 2.4 GD-6 RAIDER 4X4 A/T", "2GDD364709", 2, 1, new DateTime(2028, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 3", "LR 01 CT GP", 1, "AHTKB3CD802676867", 11, 2 },
                    { 5, 2, new DateTime(2019, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD6 RB SRX MT (Z50) 2019", "2GDC503748", 2, 1, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 4", "HY 06 DR GP", 1, "AHTJB8DB104579407", 11, 2 },
                    { 6, 2, new DateTime(2012, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC, Raider 2.7 D, 4X4, ROPS ", "2KD5635798", 1, 1, new DateTime(2028, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 5", "BT 11 SL GP", 1, "AHTFR22G906054497", 11, 2 },
                    { 7, 7, new DateTime(2005, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC", "1KD7486383", 1, 3, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 6", "WZV 941 GP", 1, "AHTEZ39G207010469", 11, 7 },
                    { 8, 2, new DateTime(2016, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC, 4X4 ( With Canopy ) ", "2GD0195958", 2, 1, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 7", "FN 57 RR GP", 1, "AHTKB3CD302604992", 11, 2 },
                    { 9, 2, new DateTime(2016, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC, 4x4", "2GD0207661", 2, 1, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 8", "FN 57 ST GP", 1, "AHTK3CD202605213", 11, 2 },
                    { 10, 2, new DateTime(2017, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "2.8 GD-6DC, 4x4 Auto ( With Canopy ) ", "1GD0252789", 2, 1, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 9", "FV 25 XR GP", 1, "AHTHA3CD403417011", 11, 2 },
                    { 11, 2, new DateTime(2017, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2 ", "2GD0284816", 1, 1, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 10", "FX 22 YD GP", 1, "AHTJB8DB504573626", 11, 2 },
                    { 12, 2, new DateTime(2017, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2 ", "2GD0284247", 1, 1, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 11", "FX 23 BX GP", 1, "AHTJB8DB304573608", 11, 2 },
                    { 13, 2, new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), " DC 2.4 GD-6RB SR MT ", "2GDC910427", 1, 1, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 12", "KP 05 MC GP", 1, "AHTJB3DD504527511", 11, 2 },
                    { 14, 2, new DateTime(2022, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), " DC 2.4 GD6 RB RAI MT ( With Canopy ) ", "2GDD036238", 2, 1, new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 13", "KW 51 TL GP", 1, "AHTJB3DD304529631", 11, 2 },
                    { 15, 2, new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), " Extra Cab HiluxXC 2.4 GD6 RBRAI 6MT ( A1P )  ", "2GDD138062", 2, 1, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 14", "LB 93 JV GP", 1, "AHTJB3DC104496782", 11, 2 },
                    { 16, 2, new DateTime(2022, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "1.8 XR CVT ", "2ZRW975634", 2, 1, new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota 15", "KV 84 DV GP ", 1, "AHTKFBAG500614236", 11, 2 },
                    { 17, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DC 2.8 GD-6RB, 4x2", "1GD0374210", 2, 1, new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S1", " HH 91 VZ GP ", 1, "AHTGA3DD900968871", 11, 2 },
                    { 18, 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2 ", "", 2, 1, new DateTime(2028, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S2", " FV 69 WP GP ", 1, "", 11, 2 },
                    { 19, 2, new DateTime(2005, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), " SC 2.4 GD-6RB, SRX 4x2", "2GD0329959", 2, 1, new DateTime(2027, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota S3", "HC 85 FY GP", 1, "AHTJB8DBX04575081", 11, 2 },
                    { 20, 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), " DC DMAX 2.2", "4JK1VT8141", 1, 1, new DateTime(2028, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Isuzu 1", " ND 836-010", 1, "ACVNRCHR3K1070230", 9, 2 }
                });

            migrationBuilder.InsertData(
                table: "LicenseDisks",
                columns: new[] { "Id", "LicenseExpiryDate", "VehicleID" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, new DateTime(2028, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, new DateTime(2023, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 7, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 8, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 9, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 10, new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 11, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 12, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 13, new DateTime(2023, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 14, new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 15, new DateTime(2028, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 16, new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 17, new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 18, new DateTime(2028, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 19, new DateTime(2027, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 20, new DateTime(2028, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ProjectId",
                table: "Booking",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StatusId",
                table: "Booking",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_VehicleId",
                table: "Booking",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseDisks_VehicleID",
                table: "LicenseDisks",
                column: "VehicleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostChecklist_VehicleId",
                table: "PostChecklist",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreChecklist_BookingID",
                table: "PreChecklist",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_StatusId",
                table: "Project",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_ProjectID",
                table: "Rate",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_RateTypeID",
                table: "Rate",
                column: "RateTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RefuelVehicle_TripId",
                table: "RefuelVehicle",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_BookingID",
                table: "Trip",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_PreChecklistId",
                table: "Trip",
                column: "PreChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_VehicleID",
                table: "Trip",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleChecklists_VehicleId",
                table: "VehicleChecklists",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModel_VehicleMakeID",
                table: "VehicleModel",
                column: "VehicleMakeID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ColourID",
                table: "Vehicles",
                column: "ColourID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FuelTypeID",
                table: "Vehicles",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_InsuranceCoverID",
                table: "Vehicles",
                column: "InsuranceCoverID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_StatusID",
                table: "Vehicles",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleMakeID",
                table: "Vehicles",
                column: "VehicleMakeID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleModelID",
                table: "Vehicles",
                column: "VehicleModelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseDisks");

            migrationBuilder.DropTable(
                name: "PostChecklist");

            migrationBuilder.DropTable(
                name: "PostDocumentation");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "RefuelVehicle");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "ServiceHistory");

            migrationBuilder.DropTable(
                name: "VehicleChecklists");

            migrationBuilder.DropTable(
                name: "RateType");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "PreChecklist");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Colour");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "InsuranceCover");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "VehicleModel");

            migrationBuilder.DropTable(
                name: "VehicleMake");
        }
    }
}

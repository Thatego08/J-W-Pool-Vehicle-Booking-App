using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Team34FinalAPI.Report_DTO_s;
using Team34FinalAPI.Services;
using Team34FinalAPI.ViewModels;
using static Team34FinalAPI.Report_DTO_s.ReportData;
using static Team34FinalAPI.ViewModels.ReportViewModel;
using OfficeOpenXml;

namespace Team34FinalAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet]
        [Route("vehicles")]
        public async Task<ActionResult<List<VehicleReportDto>>> GetVehicleReport()
        {
            var report = await _reportService.GetVehicleReport();
            return Ok(report);
        }

        [HttpGet]
        [Route("vehicle-make")]
        public async Task<IActionResult> GetVehicleMakeReport()
        {
            var report = await _reportService.GetVehicleMakeReportAsync();
            return Ok(report);
        }

        [HttpGet]
        [Route("projects")]
        public async Task<ActionResult<List<ProjectReportDto>>> GetProjectReport()
        {
            var report = await _reportService.GetProjectReport();
            return Ok(report);
        }

        [HttpGet("filtered-booking-status")]
        public async Task<ActionResult<IEnumerable<BookingStatusReportViewModel>>> GetFilteredBookingStatusReport([FromQuery] string bookingType, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var report = await _reportService.GetFilteredBookingStatusReportAsync(bookingType, startDate, endDate);
            return Ok(report);
        }

        [HttpGet("filtered-projects")]
        public async Task<ActionResult<IEnumerable<ProjectReportDto>>> GetFilteredProjects([FromQuery] string projectStatus)
        {
            var report = await _reportService.GetFilteredProjectsAsync(projectStatus);
            return Ok(report);
        }

        [HttpGet]

        [Route("vehicle-fuel-report")]
        public async Task<ActionResult<IEnumerable<FuelExpenditureReportViewModel>>> GetFuelExpendituresReport()
        {
            try
            {
                var report = await _reportService.GetFuelExpendituresReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching fuel expenditure report.");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("VehicleStatusReport")]
        public async Task<IActionResult> GetVehicleStatusReport()
        {
            try
            {
                var report = await _reportService.GetVehicleStatusReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching vehicle status report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("BookingTypeReport")]
        public async Task<IActionResult> GetBookingTypeReport()
        {
            try
            {
                var report = await _reportService.GetBookingTypeReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching booking type report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("TripReport")]
        public async Task<IActionResult> GetTripReport()
        {
            try
            {
                var report = await _reportService.GetTripReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trip report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("BookingStatusReport")]
        public async Task<IActionResult> GetBookingStatusReport()
        {
            try
            {
                var report = await _reportService.GetBookingStatusReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching booking status report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("trips-per-user-per-month")]
        public async Task<IActionResult> GetTripsPerUserPerMonth()
        {
            var report = await _reportService.GetTripsPerUserPerMonthAsync();
            return Ok(report);
        }


        [HttpGet("ProjectStatusReport")]
        public async Task<IActionResult> GetProjectStatusReport()
        {
            try
            {
                var report = await _reportService.GetProjectStatusReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project status report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        //Bookings per User per month report added
        [HttpGet("bookings-per-user-per-month")]
        public async Task<IActionResult> GetBookingsPerUserPerMonthAsync()
        {
            try
            {
                var result = await _reportService.GetBookingsPerUserPerMonthAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, "Internal server error");
            }
        }


        //Number of cancelled bookings per month
        [HttpGet("cancelled-bookings-per-month")]
        public async Task<IActionResult> GetCancelledBookingsPerMonthAsync()
        {
            try
            {
                var result = await _reportService.GetCancelledBookingsPerMonthAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("trip-duration-report")]
        public async Task<IActionResult> GetTripDurationReport()
        {
            try
            {
                var report = await _reportService.GetTripDurationReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trip duration report.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("trip-report/export")]
        public async Task<IActionResult> ExportTripReportToExcel()
        {
            try
            {
                var trips = await _reportService.GetTripDurationReportAsync();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Trip Report");

                // Headers
                worksheet.Cells[1, 1].Value = "Trip ID";
                worksheet.Cells[1, 2].Value = "Vehicle Name";
                worksheet.Cells[1, 3].Value = "Location";
                worksheet.Cells[1, 4].Value = "Booking Start";
                worksheet.Cells[1, 5].Value = "Booking End";
                worksheet.Cells[1, 6].Value = "Travel Start";
                worksheet.Cells[1, 7].Value = "Travel End";
                worksheet.Cells[1, 8].Value = "Earliest Start";
                worksheet.Cells[1, 9].Value = "Duration (hh:mm:ss)";
                worksheet.Cells[1, 10].Value = "Opening Kms";
                worksheet.Cells[1, 11].Value = "Closing Kms";
                worksheet.Cells[1, 12].Value = "Travelled Kms";
                worksheet.Cells[1, 13].Value = "Project Number"; // NEW COLUMN

                int row = 2;
                foreach (var trip in trips)
                {
                    worksheet.Cells[row, 1].Value = trip.TripId;
                    worksheet.Cells[row, 2].Value = trip.VehicleName;
                    worksheet.Cells[row, 3].Value = trip.Location;
                    worksheet.Cells[row, 4].Value = trip.BookingStart.ToString("g");
                    worksheet.Cells[row, 5].Value = trip.BookingEnd.ToString("g");
                    worksheet.Cells[row, 6].Value = trip.TravelStart.ToString("g");
                    worksheet.Cells[row, 7].Value = trip.TravelEnd.ToString("g");
                    worksheet.Cells[row, 8].Value = trip.EarliestStart.ToString("g");

                    // Duration
                    var duration = trip.Duration;
                    worksheet.Cells[row, 9].Value = $"{duration} Day{(duration == 1 ? "" : "s")}";


                    worksheet.Cells[row, 10].Value = trip.OpeningKms;
                    worksheet.Cells[row, 11].Value = trip.ClosingKms;
                    worksheet.Cells[row, 12].Value = trip.TravelledKms;
                    worksheet.Cells[row, 13].Value = trip.ProjectNumber; // NEW VALUE

                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var excelData = package.GetAsByteArray();
                return File(excelData,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "TripReport.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting trip report to Excel.");
                return StatusCode(500, "Internal server error.");
            }
        }


        //Available vehicles for the month
        //[HttpGet("available-vehicles-for-month")]
        //public async Task<IActionResult> GetAvailableVehiclesForMonthAsync()
        //{
        //    try
        //    {
        //        var result = await _reportService.GetAvailableVehiclesForMonthAsync();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception (consider using a logging framework)
        //        return StatusCode(500, "Internal server error");
        //    }
        //}


    }
}

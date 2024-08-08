using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Services;
using Team34FinalAPI.ViewModels;
using static Team34FinalAPI.Report_DTO_s.ReportData;

namespace Team34FinalAPI.Controllers
{
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

        /*  [HttpGet]
          [Route("booking-types")]
          public async Task<ActionResult<List<BookingTypeReportDto>>> GetBookingTypeReport()
          {
              var report = await _reportService.GetBookingTypeReport();
              return Ok(report);
          }
  */
        /* [HttpGet]
         [Route("trips")]
         public async Task<ActionResult<TripReportDto>> GetTripReport()
         {
             var report = await _reportService.GetTripReport();
             return Ok(report);
         }
 */
        /* [HttpGet]

         [Route("booking-status")]
         public async Task<ActionResult<List<BookingStatusReportDto>>> GetBookingStatusReport()
         {
             var report = await _reportService.GetBookingStatusReport();
             return Ok(report);
         }*/

        [HttpGet]

        [Route("projects")]
        public async Task<ActionResult<List<ProjectReportDto>>> GetProjectReport()
        {
            var report = await _reportService.GetProjectReport();
            return Ok(report);
        }


        //Additions
            
        [HttpGet]
        [Route("fuel-expenditure")]
        public async Task<ActionResult<IEnumerable<ReportViewModel.FuelExpenditureReportViewModel>>> GetFuelExpenditureReport()
        {
            try
            {

                var report = await _reportService.GetFuelExpenditureReportAsync();
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


    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
   
        // DashboardController.cs
        [ApiController]
        [Route("api/[controller]")]
       // [Authorize(Roles = "Admin")]
        public class DashboardController : ControllerBase
        {
            private readonly IVehicleRepository _vehicleRepository;
            private readonly IBookingRepository _bookingRepository;
            private readonly ITripRepository _tripRepository;

            public DashboardController(IVehicleRepository vehicleRepository,
                                     IBookingRepository bookingRepository,
                                     ITripRepository tripRepository)
            {
                _vehicleRepository = vehicleRepository;
                _bookingRepository = bookingRepository;
                _tripRepository = tripRepository;
            }

            [HttpGet("metrics")]
            public async Task<IActionResult> GetDashboardMetrics()
            {
                try
                {
                    // Get total vehicles
                    var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
                    var totalVehicles = vehicles.Count();

                    // Get available vehicles (status ID 1)
                    var availableVehicles = vehicles.Count(v => v.StatusID == 1);

                    // Get vehicles in maintenance (status ID 3)
                    var maintenanceVehicles = vehicles.Count(v => v.StatusID == 3);

                    // Get active bookings (status ID 2)
                    var bookings = await _bookingRepository.GetBookingsAsync();
                    var activeBookings = bookings.Count(b => b.StatusId == 2);

                    return Ok(new
                    {
                        TotalVehicles = totalVehicles,
                        AvailableVehicles = availableVehicles,
                        MaintenanceVehicles = maintenanceVehicles,
                        ActiveBookings = activeBookings
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpGet("recent-bookings")]
            public async Task<IActionResult> GetRecentBookings()
            {
                try
                {
                    var bookings = await _bookingRepository.GetBookingsAsync();
                    var recentBookings = bookings
                        .OrderByDescending(b => b.StartDate)
                        .Take(10)
                        .Select(b => new {
                            Id = b.BookingID,
                            VehicleName = b.Vehicle?.Name,
                            Customer = b.UserName,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate,
                            Status = b.StatusId
                        });

                    return Ok(recentBookings);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

        [HttpGet("recent-vehicles")]
        public async Task<IActionResult> GetRecentlyAddedVehicles()
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
                var recentVehicles = vehicles
                    .OrderByDescending(v => v.DateAcquired)
                    .Take(4)
                    .Select(v => new
                    {
                        v.VehicleID,
                        v.Name,
                        v.RegistrationNumber,
                        v.VehicleType,
                        v.StatusID
                    });

                return Ok(recentVehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("vehicle-status-distribution")]
            public async Task<IActionResult> GetVehicleStatusDistribution()
            {
                try
                {
                    var vehicles = await _vehicleRepository.GetAllVehiclesAsync();
                    var statusDistribution = vehicles
                        .GroupBy(v => v.StatusID)
                        .Select(g => new {
                            StatusId = g.Key,
                            Count = g.Count()
                        });

                    return Ok(statusDistribution);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpGet("booking-statistics")]
            public async Task<IActionResult> GetBookingStatistics([FromQuery] int days = 30)
            {
                try
                {
                    var bookings = await _bookingRepository.GetBookingsAsync();
                    var startDate = DateTime.Now.AddDays(-days);

                    var dailyStats = bookings
                        .Where(b => b.StartDate >= startDate)
                        .GroupBy(b => b.StartDate.Date)
                        .Select(g => new {
                            Date = g.Key,
                            BookingsCount = g.Count(),
                            CompletedCount = g.Count(b => b.StatusId == 3) // Assuming 3 is completed status
                        })
                        .OrderBy(s => s.Date);

                    return Ok(dailyStats);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }


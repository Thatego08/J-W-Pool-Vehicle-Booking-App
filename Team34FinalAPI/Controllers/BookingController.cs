using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Services;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly BookingDbContext _context;
        private readonly VehicleDbContext _vehicleContext;
        private readonly ILogger<BookingController> _logger;
        private readonly IEmailService _emailService;

        public BookingController(IBookingRepository bookingRepository, IVehicleRepository vehicleRepository, IProjectRepository projectRepository, BookingDbContext context, VehicleDbContext vehicleContext, ILogger<BookingController> logger, IEmailService emailService)
        {
            _bookingRepository = bookingRepository;
            _projectRepository = projectRepository;
            _vehicleRepository = vehicleRepository;
            _context = context;
            _vehicleContext = vehicleContext;
            _logger = logger;
            _emailService = emailService;
        }

        // Get all bookings
        [HttpGet]
        [Route("GetAllBookings")]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _bookingRepository.GetBookingsAsync();
                var bookingViewModels = MapToViewModel(bookings);
                return Ok(bookingViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookings");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get specific Booking
        [HttpGet("GetBooking{id}")]
        public async Task<ActionResult<BookingViewModel>> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(MapToViewModel(booking));
        }

        // Updated Post method, to include 'Send Booking Confirmation'
        [HttpPost]
        [Route("AddBooking")]
        public async Task<ActionResult<BookingViewModel>> PostBookingAsync(BookingViewModel bookingViewModel)
        {
            //Add Booking Adjustments

            _logger.LogInformation("Entering PostBookingAsync with data: {@BookingViewModel}", bookingViewModel);
            try
            {
                // Validate vehicle
                _logger.LogInformation("Finding vehicle: {VehicleName}", bookingViewModel.VehicleName);
                var vehicle = await _vehicleRepository.GetVehicleByNameAsync(bookingViewModel.VehicleName);
                if (vehicle == null)
                {
                    _logger.LogWarning("Vehicle with name {VehicleName} does not exist.", bookingViewModel.VehicleName);
                    return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} does not exist.");
                }

                if (vehicle.StatusID == 2 || vehicle.StatusID == 3)
                {
                    _logger.LogWarning("Vehicle with name {VehicleName} is not available for booking.", bookingViewModel.VehicleName);
                    return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} is not available for booking.");
                }

                // Validate project or event
                int? projectId = null;
                string? eventName = null;

                if (bookingViewModel.ProjectNumber.HasValue)
                {
                    _logger.LogInformation("Finding project: {ProjectNumber}", bookingViewModel.ProjectNumber.Value);
                    var project = await _projectRepository.GetProjectByNumberAsync(bookingViewModel.ProjectNumber.Value);
                    if (project == null)
                    {
                        _logger.LogWarning("Project with number {ProjectNumber} does not exist.", bookingViewModel.ProjectNumber.Value);
                        return BadRequest($"Project with number {bookingViewModel.ProjectNumber.Value} does not exist.");
                    }
                    projectId = project.ProjectID;
                }
                else if (!string.IsNullOrEmpty(bookingViewModel.Event))
                {
                    eventName = bookingViewModel.Event;
                }
                else
                {
                    _logger.LogWarning("Either ProjectNumber or Event must be provided.");
                    return BadRequest("Either ProjectNumber or Event must be provided.");
                }

                // Create booking
                var booking = new Booking
                {
                    UserName = bookingViewModel.UserName,
                    Event = bookingViewModel.Event,
                    Type = bookingViewModel.Type,
                    StartDate = bookingViewModel.StartDate,
                    EndDate = bookingViewModel.EndDate,
                    VehicleId = vehicle.VehicleID,
                    ProjectId = projectId,
                    StatusId = 2
                };

                _logger.LogInformation("Booking details before adding: {@Booking}", booking);

                vehicle.StatusID = 2;

                _logger.LogInformation("Adding booking to the repository.");
                await _bookingRepository.AddBookingAsync(booking);

                _logger.LogInformation("Updating vehicle status.");
                await _vehicleRepository.UpdateVehicleAsync(vehicle);

                bookingViewModel.BookingID = booking.BookingID;

                var message = $"Dear {booking.UserName},<br/><br/>Your booking has been confirmed for {booking.StartDate} with the vehicle {vehicle.Name}.<br/><br/>Thank you,<br/>Jones and Wagener International";

                _logger.LogInformation("Sending confirmation email to {UserName}", booking.UserName);
                await _emailService.SendEmailAsync(booking.UserName, "Booking Confirmation", message);

                _logger.LogInformation("Booking created with ID {BookingID}", booking.BookingID);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, bookingViewModel);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException in PostBookingAsync: {InnerExceptionMessage}", ex.InnerException?.Message ?? ex.Message);
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in PostBookingAsync");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            /*_logger.LogInformation("Entering PostBookingAsync with data: {@BookingViewModel}", bookingViewModel);
            try
            {
                // Validate vehicle
                _logger.LogInformation("Finding vehicle: {VehicleName}", bookingViewModel.VehicleName);
                var vehicle = await _vehicleContext.Vehicles.SingleOrDefaultAsync(v => v.Name == bookingViewModel.VehicleName);
                if (vehicle == null)
                {
                    _logger.LogWarning("Vehicle with name {VehicleName} does not exist.", bookingViewModel.VehicleName);
                    return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} does not exist.");
                }

                if (vehicle.StatusID == 2 || vehicle.StatusID == 3)
                {
                    _logger.LogWarning("Vehicle with name {VehicleName} is not available for booking.", bookingViewModel.VehicleName);
                    return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} is not available for booking.");
                }

                // Validate project or event
                int? projectId = null;
                string? eventName = null;

                if (bookingViewModel.ProjectNumber.HasValue)
                {
                    _logger.LogInformation("Finding project: {ProjectNumber}", bookingViewModel.ProjectNumber.Value);
                    var project = await _context.Projects.SingleOrDefaultAsync(p => p.ProjectNumber == bookingViewModel.ProjectNumber.Value);
                    if (project == null)
                    {
                        _logger.LogWarning("Project with number {ProjectNumber} does not exist.", bookingViewModel.ProjectNumber.Value);
                        return BadRequest($"Project with number {bookingViewModel.ProjectNumber.Value} does not exist.");
                    }
                    projectId = project.ProjectID;
                }
                else if (!string.IsNullOrEmpty(bookingViewModel.Event))
                {
                    eventName = bookingViewModel.Event;
                }
                else
                {
                    _logger.LogWarning("Either ProjectNumber or Event must be provided.");
                    return BadRequest("Either ProjectNumber or Event must be provided.");
                }

                // Create booking
                var booking = new Booking
                {
                    UserName = bookingViewModel.UserName,
                    Event = eventName,
                    StartDate = bookingViewModel.StartDate,
                    EndDate = bookingViewModel.EndDate,
                    VehicleId = vehicle.VehicleID,
                    ProjectId = projectId
                };

                _logger.LogInformation("Booking details before adding: {@Booking}", booking);

                vehicle.StatusID = 2;

                _logger.LogInformation("Adding booking to the repository.");
                await _bookingRepository.AddBookingAsync(booking);

                _logger.LogInformation("Updating vehicle status.");
                _vehicleContext.Vehicles.Update(vehicle);
                await _vehicleContext.SaveChangesAsync();

                bookingViewModel.BookingID = booking.BookingID;

                var message = $"Dear {booking.UserName},<br/><br/>Your booking has been confirmed for {booking.StartDate} with the vehicle {vehicle.Name}.<br/><br/>Thank you,<br/>Jones and Wagener International";

                _logger.LogInformation("Sending confirmation email to {UserName}", booking.UserName);
                await _emailService.SendEmailAsync(booking.UserName, "Booking Confirmation", message);

                _logger.LogInformation("Booking created with ID {BookingID}", booking.BookingID);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, bookingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostBookingAsync");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }*/
        }


        // Edit booking
        [HttpPut("EditBooking")]
        public async Task<IActionResult> PutBookingAsync(int id, BookingViewModel bookingViewModel)
        {
            // Log the request data
            _logger.LogInformation("Entering PutBookingAsync with ID: {Id} and data: {@BookingViewModel}", id, bookingViewModel);

            // Validate the ID in the request body
            if (id != bookingViewModel.BookingID)
            {
                _logger.LogWarning("ID in URL does not match ID in body.");
                return BadRequest("ID in URL does not match ID in body.");
            }

            // Fetch the existing booking
            var booking = await _context.Bookings
                                        .Include(b => b.Vehicle)
                                        .Include(b => b.Project)
                                        .FirstOrDefaultAsync(b => b.BookingID == id);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID {Id} not found.", id);
                return NotFound();
            }

            // Validate and update the vehicle if needed
            if (!string.IsNullOrEmpty(bookingViewModel.VehicleName))
            {
                var vehicle = await _context.Vehicles
                                            .FirstOrDefaultAsync(v => v.Name == bookingViewModel.VehicleName);
                if (vehicle == null)
                {
                    _logger.LogWarning("Vehicle with name {VehicleName} does not exist.", bookingViewModel.VehicleName);
                    return BadRequest("Invalid Vehicle");
                }
                booking.Vehicle = vehicle;
            }

            // Validate and update the project if needed
            if (bookingViewModel.ProjectNumber.HasValue)
            {
                var project = await _context.Projects
                                            .FirstOrDefaultAsync(p => p.ProjectNumber == bookingViewModel.ProjectNumber.Value);
                if (project == null)
                {
                    _logger.LogWarning("Project with number {ProjectNumber} does not exist.", bookingViewModel.ProjectNumber.Value);
                    return BadRequest("Invalid Project");
                }
                booking.Project = project;
            }

            // Update booking details
            booking.UserName = bookingViewModel.UserName;
            booking.Event = bookingViewModel.Event;
            booking.StartDate = bookingViewModel.StartDate;
            booking.EndDate = bookingViewModel.EndDate;

            // Log the booking details before saving
            _logger.LogInformation("Updated booking details: {@Booking}", booking);

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    _logger.LogWarning("Booking with ID {Id} not found during update.", id);
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }

        // Delete Booking
        [HttpDelete("DeleteBooking{id}")]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();

            await _bookingRepository.DeleteBookingAsync(id);

            return NoContent();
        }

        // Search booking history by username
        [HttpGet("SearchBookingHistory/{username}")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> SearchBookingHistoryAsync(string username)
        {
            var bookings = await _bookingRepository.GetBookingsByUserNameAsync(username);

            if (bookings == null || !bookings.Any())
            {
                return NotFound("Booking History Does Not Exist");
            }

            var bookingViewModels = bookings.Select(b => new BookingViewModel
            {
                BookingID = b.BookingID,
                UserName = b.UserName,
                Event = b.Event,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                VehicleName = b.Vehicle?.Name,
                ProjectNumber = b.Project?.ProjectNumber
            }).ToList();

            return Ok(bookingViewModels);
        }

        private List<BookingViewModel> MapToViewModel(IEnumerable<Booking> bookings)
        {
            var bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                var vehicleName = booking.Vehicle?.Name ?? "Unknown Vehicle";
                var projectNumber = booking.Project?.ProjectNumber != null ? (int?)booking.Project.ProjectNumber : null;

                var bookingViewModel = new BookingViewModel
                {
                    BookingID = booking.BookingID,
                    UserName = booking.UserName,
                    Event = booking.Event,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    VehicleName = vehicleName,
                    ProjectNumber = projectNumber
                };

                bookingViewModels.Add(bookingViewModel);
            }

            return bookingViewModels;
        }

        private BookingViewModel MapToViewModel(Booking booking)
        {
            return new BookingViewModel
            {
                BookingID = booking.BookingID,
                UserName = booking.UserName,
                Event = booking.Event,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                VehicleName = booking.Vehicle?.Name ?? "Unknown Vehicle",
                ProjectNumber = booking.Project?.ProjectNumber
            };
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Team34FinalAPI.Services;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingDbContext _context;
        private readonly VehicleDbContext _vehicleContext;
        private readonly ILogger<BookingController> _logger;
        private readonly IEmailService _emailService;

        public BookingController(IBookingRepository bookingRepository, BookingDbContext context, VehicleDbContext vehicleContext , ILogger<BookingController> logger, IEmailService emailService)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _vehicleContext = vehicleContext;
            _logger = logger;
            _emailService = emailService;
        }

        // Get all bookings
        [HttpGet]
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
                Console.WriteLine($"Error in GetBookings: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // Get specific Booking
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingViewModel>> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // Updated Post method, to include 'Send Booking Confirmation'
        [HttpPost]
        public async Task<ActionResult<BookingViewModel>> PostBookingAsync(BookingViewModel bookingViewModel)
        {
            _logger.LogInformation("Entering PostBookingAsync with data: {@BookingViewModel}", bookingViewModel);
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

                if (!string.IsNullOrEmpty(bookingViewModel.ProjectName))
                {
                    _logger.LogInformation("Finding project: {ProjectName}", bookingViewModel.ProjectName);
                    var project = await _context.Projects.SingleOrDefaultAsync(p => p.ProjectName == bookingViewModel.ProjectName);
                    if (project == null)
                    {
                        _logger.LogWarning("Project with name {ProjectName} does not exist.", bookingViewModel.ProjectName);
                        return BadRequest($"Project with name {bookingViewModel.ProjectName} does not exist.");
                    }
                    projectId = project.ProjectID;
                }
                else if (!string.IsNullOrEmpty(bookingViewModel.Event))
                {
                    eventName = bookingViewModel.Event;
                }
                else
                {
                    _logger.LogWarning("Either ProjectName or Event must be provided.");
                    return BadRequest("Either ProjectName or Event must be provided.");
                }

                // Create booking
                var booking = new Booking
                {
                    UserName = bookingViewModel.UserName,
                    Event = eventName,
                    StartDate = bookingViewModel.StartDate,
                    EndDate = bookingViewModel.EndDate,
                    VehicleId = vehicle.VehicleID,
                    ProjectId = projectId,
                    RateType = bookingViewModel.RateType
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
            }
        }



        //Testing confirmation
        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                await _emailService.SendEmailAsync("test@example.com", "Test Email", "This is a test email.");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // Edit booking
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingAsync(int id, BookingViewModel bookingViewModel)
        {
            // If booking Id in url does not match booking id in request body, return Bad request response 400
            if (id != bookingViewModel.BookingID) return BadRequest();

            // Find the existing booking
            var booking = await _context.Bookings
                                        .Include(b => b.Vehicle)
                                        .Include(b => b.Project)
                                        .FirstOrDefaultAsync(b => b.BookingID == id);
            if (booking == null) return NotFound();

            // Find the associated Vehicle
            Vehicle vehicle = null;
            if (!string.IsNullOrEmpty(bookingViewModel.VehicleName))
            {
                vehicle = await _context.Vehicles
                                        .FirstOrDefaultAsync(v => v.Name == bookingViewModel.VehicleName);
                if (vehicle == null) return BadRequest("Invalid Vehicle");
            }

            // Find the associated Project
            Project project = null;
            if (!string.IsNullOrEmpty(bookingViewModel.ProjectName))
            {
                project = await _context.Projects
                                        .FirstOrDefaultAsync(p => p.ProjectName == bookingViewModel.ProjectName);
                if (project == null) return BadRequest("Invalid Project");
            }

            // Update booking properties
            booking.UserName = bookingViewModel.UserName;
            booking.Event = bookingViewModel.Event;
            booking.StartDate = bookingViewModel.StartDate;
            booking.EndDate = bookingViewModel.EndDate;
            booking.Vehicle = vehicle ?? booking.Vehicle;
            booking.Project = project ?? booking.Project;
            booking.RateType = bookingViewModel.RateType;

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Return 404 Not found response if booking does not exist
                if (!BookingExists(id)) return NotFound();
                throw;
            }

            // Return 204 No Content response indicating that the update was successful
            return NoContent();
        }

        // Helper method to check if a booking exists by Id
        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }


        // Delete Booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            // Find booking to be deleted by Id
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();


            // Remove booking from context
            await _bookingRepository.DeleteBookingAsync(id);

            // Return 204 No Content response indicating that the deletion was successful
            return NoContent();
        }



        // Search booking history by username
        [HttpGet("history/{username}")]
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
                ProjectName = b.Project?.ProjectName
            }).ToList();

            return Ok(bookingViewModels);
        }

        private List<BookingViewModel> MapToViewModel(IEnumerable<Booking> bookings)
        {
            var bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                var vehicleName = booking.Vehicle?.Name ?? "Unknown Vehicle";
                var projectName = booking.Project?.ProjectName ?? "No Project";

                var bookingViewModel = new BookingViewModel
                {
                    BookingID = booking.BookingID,
                    UserName = booking.UserName,
                    Event = booking.Event,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    VehicleName = vehicleName,
                    ProjectName = projectName,
                    RateType = booking.RateType
                };

                Console.WriteLine($"BookingID: {booking.BookingID}, RateType: {bookingViewModel.RateType}");

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
                VehicleName = booking.Vehicle?.Name ?? "Unknown Vehicle", // Handle null vehicle
                ProjectName = booking.Project?.ProjectName ?? "No Project", // Handle null project
                RateType = booking.RateType
            };
        }
    }
}
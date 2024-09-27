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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Mailjet.Client.Resources;
using User = Team34FinalAPI.Models.User;

namespace Team34FinalAPI.Controllers
{
    [Authorize]
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

        private readonly UserManager<User> _userManager;

        public BookingController(IBookingRepository bookingRepository, UserManager<User> userManager, IVehicleRepository vehicleRepository, IProjectRepository projectRepository, BookingDbContext context, VehicleDbContext vehicleContext, ILogger<BookingController> logger, IEmailService emailService)
        {
            _bookingRepository = bookingRepository;
            _projectRepository = projectRepository;
            _vehicleRepository = vehicleRepository;
            _context = context;
            _vehicleContext = vehicleContext;
            _logger = logger;
            _emailService = emailService;

            _userManager = userManager;
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
        [HttpGet("GetBooking/{id}")]
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
            _logger.LogInformation("Entering PostBookingAsync with data: {@BookingViewModel}", bookingViewModel);
            try
            {
                // Validate vehicle
                var vehicle = await _vehicleRepository.GetVehicleByNameAsync(bookingViewModel.VehicleName);
                if (vehicle == null)
                {
                    return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} does not exist.");
                }

                // Validate project or event
                int? projectId = null;
                if (bookingViewModel.ProjectNumber.HasValue)
                {
                    var project = await _projectRepository.GetProjectByNumberAsync(bookingViewModel.ProjectNumber.Value);
                    if (project == null)
                    {
                        return BadRequest($"Project with number {bookingViewModel.ProjectNumber.Value} does not exist.");
                    }
                    projectId = project.ProjectID;
                }
                _logger.LogInformation("Starting SomeAction for user: {User}", User.Identity.Name);


                // Check if the vehicle is available for the selected date range
                var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(bookingViewModel.StartDate, bookingViewModel.EndDate);

                // Validate the selected vehicle
                var selectedVehicle = await _vehicleRepository.GetVehicleByNameAsync(bookingViewModel.VehicleName);
                if (selectedVehicle == null || !availableVehicles.Any(v => v.VehicleID == selectedVehicle.VehicleID))
                {
                    return BadRequest("The selected vehicle is not available during the selected date range.");
                }

               // Check if the vehicle is already booked within the provided date range
                var conflictingBooking = await _bookingRepository.GetConflictingBookingAsync(vehicle.VehicleID, bookingViewModel.StartDate, bookingViewModel.EndDate);
                if (conflictingBooking != null)
                {
                    return BadRequest("The vehicle is not available during the selected date range.");
                }


                // Retrieve the logged-in user



                // Get the username of the logged-in user
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest(new { message = "User not found." });
                }

                // Retrieve the user profile based on the username
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                //var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(); // No logged-in user found
                }

                _logger.LogInformation("User retrieved successfully: Username:", user.UserName);


               

                // Proceed with booking creation
                var booking = new Booking
                {
                    UserName = bookingViewModel.UserName,
                    Event = bookingViewModel.Event,
                    Type = bookingViewModel.Type,
                    StartDate = bookingViewModel.StartDate,
                    EndDate = bookingViewModel.EndDate,  // Add end date
                    VehicleId = vehicle.VehicleID,
                    ProjectId = projectId,
                    StatusId = 2 // Assuming '2' means 'Booked'
                };

             

                await _bookingRepository.AddBookingAsync(booking);


                await _vehicleRepository.UpdateVehicleAsync(vehicle);

                bookingViewModel.BookingID = booking.BookingID;

                // Send confirmation email (as implemented)
                // After booking is successfully created, send the confirmation email
                string subject = "Booking Confirmation";
                string message = $"Dear {user.Name},\n\nYour booking has been successfully created. Details:\n\n" +
                                 $"Booking Date: {bookingViewModel.StartDate}\n" +
                                 $"Vehicle: {bookingViewModel.VehicleName}\n" +
                                 $"Type: {bookingViewModel.Type}\n\n" +
                                 "Thank you for using our service.";


                await _emailService.SendEmailAsync(user.Email, subject, message);

                return Ok(new { message = "Booking created and confirmation email sent." });
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingID }, bookingViewModel);

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "DbUpdateException in PostBookingAsync");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in PostBookingAsync");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        [HttpGet("GetAvailableVehicles")]
        public async Task<IActionResult> GetAvailableVehiclesAsync(DateTime startDate, DateTime endDate)
        {
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(startDate, endDate);
            return Ok(availableVehicles);
        }



        // Cancel Booking
        [HttpPut("CancelBooking/{id}")]
        public async Task<IActionResult> CancelBookingAsync(int id)
        {
            try
            {
                // Fetch the booking
                var booking = await _bookingRepository.GetBookingByIdAsync(id);
                if (booking == null)
                {
                    return NotFound("Booking not found.");
                }

                // Fetch the associated vehicle
                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(booking.VehicleId);
                if (vehicle == null)
                {
                    return NotFound("Associated vehicle not found.");
                }

                // Update the status of the vehicle to 'Available'
                vehicle.StatusID = 1; // 1 corresponds to 'Available'
                await _vehicleRepository.UpdateVehicleAsync(vehicle);

                // Update the status of the booking to 'Cancelled'
                booking.StatusId = 4; // 4 corresponds to 'Cancelled'
                await _bookingRepository.UpdateBookingAsync(booking);

                // Save changes
                await _context.SaveChangesAsync();

                // Optional: You can also send a notification email to the user about the cancellation.
                var user = await _userManager.FindByNameAsync(booking.UserName);
                if (user != null)
                {
                    string subject = "Booking Cancellation";
                    string message = $"Dear {user.Name},\n\nYour booking for vehicle {vehicle.Name} has been successfully cancelled.\n\nThank you.";
                    await _emailService.SendEmailAsync(user.Email, subject, message);
                }

                //return Ok("Booking cancelled and vehicle status updated to 'Available'.");
                return Ok(new { message = "Booking cancelled and vehicle status updated to 'Available'." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelBookingAsync");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateVehicleStatus/{vehicleName}")]
        public async Task<IActionResult> UpdateVehicleStatus(string vehicleName, [FromBody] UpdateVehicleStatusDTO statusDto)
        {
            if (string.IsNullOrWhiteSpace(vehicleName) || statusDto == null)
            {
                return BadRequest("Invalid vehicle name or status data.");
            }

            // Find the vehicle by its name
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Name == vehicleName);

            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            // Update the vehicle status
            vehicle.StatusID = statusDto.StatusId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Vehicle status updated successfully." });
            }
            catch (DbUpdateException)
            {
                // Handle the case where the update fails
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the vehicle status.");
            }
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

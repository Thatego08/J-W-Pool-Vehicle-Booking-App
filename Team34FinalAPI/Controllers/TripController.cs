using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly TripDbContext _context;
        private readonly ITripRepository _tripRepository;
        private readonly IAuditLogRepository _auditLogRepo;

        public TripController(TripDbContext context, ITripRepository tripRepository, IAuditLogRepository auditLogRepo)
        {
            _context = context;
            _tripRepository = tripRepository;
            _auditLogRepo = auditLogRepo;
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("createTrip")]
        public async Task<IActionResult> CreateTrip([FromForm] TripViewModel tvm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Message = "Model validation failed", Errors = errors });
            }

            if (tvm == null)
            {
                return BadRequest("TripViewModel cannot be null");
            }

            // Get the current logged-in user's username
            var userName = User.Identity.Name;

            // Check if BookingID exists
            var bookingExists = await _context.Bookings.AnyAsync(b => b.BookingID == tvm.BookingID);
            if (!bookingExists)
            {
                return BadRequest("Invalid BookingID.");
            }

            // Check if PreChecklistId is valid (if provided)
            if (tvm.PreChecklistId.HasValue)
            {
                var preChecklistExists = await _context.PreChecklists.AnyAsync(pc => pc.Id == tvm.PreChecklistId.Value);
                if (!preChecklistExists)
                {
                    return BadRequest("Invalid PreChecklistId.");
                }
            }

            var trip = new Trip
            {
                Name = tvm.Name,
                Location = tvm.Location,
                BookingID = tvm.BookingID,
                Comment = tvm.Comment,
                TravelStart = tvm.TravelStart,
                TravelEnd = tvm.TravelEnd,
                UserName = userName,
                PreChecklistId = tvm.PreChecklistId // Set the PreChecklistId
            };

            try
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();

                //Audit Log stuff 
                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = userName,
                    Action = "Create New Trip",
                    Details = $"Trip created for vehicle booking. Trip created by : " + userName +" with the following comments: "+trip.Comment,
                    Timestamp = DateTime.UtcNow
                });

            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Internal server error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(trip);
        }



        [Authorize(Roles = "Driver")]
        [HttpPut("UpdateTrip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripViewModel tvm)
        {
            if (id != tvm.TripId)
            {
                return BadRequest("Trip ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound($"Trip with ID {id} not found");
            }

            // Update trip properties
            trip.Name = tvm.Name; // Ensure the 'Name' property exists in your Trip model
            trip.Location = tvm.Location;
            trip.Comment = tvm.Comment;
            trip.TravelStart = tvm.TravelStart;
            trip.TravelEnd = tvm.TravelEnd;

            try
            {
                await _context.SaveChangesAsync();
                //Audit Log stuff 
                await _auditLogRepo.AddLogAsync(new AuditLog
                {
                    UserName = trip.UserName,
                    Action = "Update Trip",
                    Details = $"Trip details for "+ tvm.Name + " have been updated by: " + trip.UserName,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    // Log the error and return a successful response with an error message
                    return Ok(new { Message = "Trip not found during update" });
                }
                else
                {
                    // Rethrow the exception if there is another issue
                    throw;
                }
            }

            // Return a successful response indicating the update was successful
            return Ok(new { Message = "Trip updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips()
        {
            try
            {
                // Fetch trips without including related TripMedia
                var trips = await _context.Trips
                    .Include(t => t.Booking)         // Include related Booking if necessary
                    .Include(t => t.PreChecklist)    // Include related PreChecklist
                    .Include(t => t.RefuelVehicles)  // Include related RefuelVehicles if necessary
                    .ToListAsync();

                return Ok(trips);
            }
            catch (Exception ex)
            {
                // Log error details (if you have logging set up)
                // _logger.LogError(ex, "An error occurred while retrieving trips.");

                // Return a general error message
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetTripById/{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Booking)         // Booking can be null
                .Include(t => t.PreChecklist)    // PreChecklist can be null
                .Include(t => t.RefuelVehicles)  // RefuelVehicles can be null or empty list
                .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound($"Trip with ID {id} not found");
            }

            // You can explicitly check for null related data if necessary
            trip.Booking = trip.Booking ?? new Booking();
            trip.PreChecklist = trip.PreChecklist ?? new PreChecklist();
            trip.RefuelVehicles = trip.RefuelVehicles ?? new List<RefuelVehicle>();

            return Ok(trip);
        }



        [Authorize(Roles = "Driver")]
        [HttpGet("GetPreviousTripsByUserName/{userName}")]
        public async Task<IActionResult> GetPreviousTripsByUserName(string userName)
        {
            var currentUserName = User.Identity.Name; // Extract the username from the token

            if (currentUserName != userName)
            {
                return Unauthorized("You are not authorized to view other users' trips.");
            }

            var trips = await _context.Trips
                .Include(t => t.Booking)         // Include related Booking if necessary
                .Include(t => t.PreChecklist)    // Include related PreChecklist
                .Include(t => t.RefuelVehicles)  // Include related RefuelVehicles if necessary
                .Where(t => t.UserName == userName)
                .ToListAsync();

            if (trips == null || !trips.Any())
            {
                return NotFound($"No trips found for user {userName}");
            }

            return Ok(trips);
        }


        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.TripId == id);
        }

        [HttpDelete]
        [Route("DeleteTrip/{TripId}")]
        public async Task<IActionResult> DeleteTrip(int TripId)
        {
            try
            {
                var existingTrip = await _tripRepository.GetTripByIdAsync(TripId);
                if (existingTrip == null)
                    return NotFound($"The trip with ID {TripId} does not exist");

                _tripRepository.Delete(existingTrip);

                if (await _tripRepository.SaveChangesAsync())

                {

                    //Audit Log stuff 
                    await _auditLogRepo.AddLogAsync(new AuditLog
                    {
                       Action = "Delete Trip",
                        Details = $"Trip has been deleted by an administrator" ,
                        Timestamp = DateTime.UtcNow
                    });
                    return Ok(existingTrip);

                }

                return StatusCode(500, "Failed to delete trip");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

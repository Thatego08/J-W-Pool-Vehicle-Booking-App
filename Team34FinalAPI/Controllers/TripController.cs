using iText.Kernel.Counter.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

using Team34FinalAPI.Tools;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly TripDbContext _context;
        private readonly ITripRepository _tripRepository;

        public TripController(TripDbContext context, ITripRepository tripRepository)
        {
            _context = context;
            _tripRepository = tripRepository;
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("createTrip")]
        public async Task<IActionResult> CreateTrip([FromForm] TripViewModel tvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (tvm == null)
            {
                return BadRequest("TripViewModel cannot be null");
            }

            // Get the current logged-in user's username
            var userName = User.Identity.Name;

            var trip = new Trip
            {
                VehicleId = tvm.VehicleId,
                Location = tvm.Location,
                FuelAmount = tvm.FuelAmount,
                Comment = tvm.Comment,
                TravelStart = tvm.TravelStart,
                TravelEnd = tvm.TravelEnd,
                RegistrationNumber = tvm.RegistrationNumber,
                UserName = userName // Assign the username
            };

            // Initialize TripMedia collection if it's null
            trip.TripMedia = new List<TripMedia>();

            // Handle file uploads if there are any
            if (tvm.MediaFiles != null && tvm.MediaFiles.Any())
            {
                foreach (var file in tvm.MediaFiles)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();

                        var tripMedia = new TripMedia
                        {
                            Trip = trip,
                            Description = tvm.MediaDescription, // Nullable description
                            FileName = file.FileName,
                            FileContent = fileBytes,
                            MediaType = file.ContentType
                        };

                        trip.TripMedia.Add(tripMedia);
                    }
                }
            }

            try
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> UpdateTrip(int id, [FromForm] TripViewModel tvm)
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
                .Include(t => t.TripMedia)
                .FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound($"Trip with ID {id} not found");
            }

            // Update trip properties
            trip.VehicleId = tvm.VehicleId;
            trip.Location = tvm.Location;
            trip.FuelAmount = tvm.FuelAmount;
            trip.Comment = tvm.Comment;
            trip.TravelStart = tvm.TravelStart;
            trip.TravelEnd = tvm.TravelEnd;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTrips")]
        public async Task<IActionResult> GetAllTrips()
        {
            var trips = await _context.Trips.Include(t => t.TripMedia).ToListAsync();
            return Ok(trips);
        }

        [Authorize(Roles = "Driver,Admin")]
        [HttpGet("GetTripById/{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _context.Trips.Include(t => t.TripMedia).FirstOrDefaultAsync(t => t.TripId == id);

            if (trip == null)
            {
                return NotFound($"Trip with ID {id} not found");
            }

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
                .Include(t => t.TripMedia)
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
                    return Ok(existingTrip);

                return StatusCode(500, "Failed to delete trip");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



       
        [HttpPost("{tripId}/RefuelVehicle")]
        public async Task<IActionResult> AddRefuelVehicle(int tripId, [FromBody] RefuelVehicle refuelVehicle)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
            {
                return NotFound($"Trip with ID {tripId} not found.");
            }

            refuelVehicle.TripId = tripId;

            _context.RefuelVehicles.Add(refuelVehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRefuelVehicle), new { tripId, refuelVehicleId = refuelVehicle.RefuelVehicleId }, refuelVehicle);
        }


        [HttpGet("{tripId}/RefuelVehicle/{refuelVehicleId}")]
        public async Task<IActionResult> GetRefuelVehicle(int tripId, int refuelVehicleId)
        {
            var refuelVehicle = await _context.RefuelVehicles
                .Where(rv => rv.TripId == tripId && rv.RefuelVehicleId == refuelVehicleId)
                .FirstOrDefaultAsync();

            if (refuelVehicle == null)
            {
                return NotFound($"RefuelVehicle with ID {refuelVehicleId} for Trip ID {tripId} not found.");
            }

            return Ok(refuelVehicle);
        }

        // Similar methods for Update and Delete operations
    }
}
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

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly TripDbContext _context;

        public TripController(TripDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Driver,Admin")]
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

            var trip = new Trip
            {
                VehicleId = tvm.VehicleId,
                Location = tvm.Location,
                FuelAmount = tvm.FuelAmount,
                Comment = tvm.Comment,
                TravelStart = tvm.TravelStart,
                TravelEnd = tvm.TravelEnd
            };

            // Initialize TripMedia collection if it's null
            trip.TripMedia = new List<TripMedia>();

            // Handle file uploads
            if (tvm.MediaFiles != null)
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
                            Description = tvm.MediaDescription,
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

        [Authorize(Roles = "Driver,Admin")]
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

        [Authorize(Roles = "Driver,Admin")]
        [HttpGet("GetTripsByDriver/{driverId}")]
    public async Task<IActionResult> GetTripsByDriver(int driverId)
    {
        var trips = await _context.Trips
            .Include(t => t.TripMedia)
            .Where(t => t.VehicleId == driverId)
            .ToListAsync();

        if (trips == null || !trips.Any())
        {
            return NotFound($"No trips found for driver with ID {driverId}");
        }

        return Ok(trips);
    }

    private bool TripExists(int id)
    {
        return _context.Trips.Any(e => e.TripId == id);
    }
}
}

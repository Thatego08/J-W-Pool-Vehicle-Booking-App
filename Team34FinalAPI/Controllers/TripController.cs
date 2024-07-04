using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Team34FinalAPI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Team34FinalAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                TravelEnd = tvm.TravelEnd,
                RegistrationNumber = tvm.RegistrationNumber
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
                // Log the inner exception message
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Internal server error: {innerExceptionMessage}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(trip);
        }


        [HttpPut("UpdateTrip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, TripViewModel tvm)
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
                .Include(t => t.TripMedia) // Include TripMedias if needed
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

            // Handle media files if necessary
            // ...

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

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.TripId == id);
        }
    }
}
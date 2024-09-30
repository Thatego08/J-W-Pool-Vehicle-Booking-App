using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreChecklistController : ControllerBase
    {
        private readonly TripDbContext _context;

        public PreChecklistController(TripDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PreChecklist>> GetPreChecklist(int id)
        {
            var preChecklist = await _context.PreChecklists.FindAsync(id);

            if (preChecklist == null)
            {
                return NotFound();
            }

            return Ok(preChecklist);
        }
        [HttpPost]
        public async Task<ActionResult> CreatePreChecklist([FromBody] PreChecklist preChecklist)
        {
            if (preChecklist == null)
            {
                return BadRequest("PreChecklist cannot be null.");
            }

            // Check if BookingID is valid (optional)
            if (preChecklist.BookingID <= 0)
            {
                return BadRequest("Invalid BookingID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assign the Booking based on BookingID
            var booking = await _context.Bookings.FindAsync(preChecklist.BookingID);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            preChecklist.Booking = booking;

            // Add the new PreChecklist and save changes
            _context.PreChecklists.Add(preChecklist);
            await _context.SaveChangesAsync();

            // Return the Id in the response
            return Ok(new { id = preChecklist.Id });
        }


        // private bool PreChecklistExists(int tripId)
        // {
        //   return _context.PreChecklists.Any(e => e.TripId == tripId);
        // }
    }
}
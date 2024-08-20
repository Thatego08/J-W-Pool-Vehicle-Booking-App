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

        [HttpGet("{tripId}")]
        public async Task<ActionResult<PreChecklist>> GetPreChecklist(int tripId)
        {
            var preChecklist = await _context.PreChecklists
                .FirstOrDefaultAsync(pc => pc.TripId == tripId);

            if (preChecklist == null)
            {
                return NotFound();
            }

            return preChecklist;
        }

        [HttpPost]
        public async Task<ActionResult<PreChecklist>> CreatePreChecklist(PreChecklist preChecklist)
        {
            _context.PreChecklists.Add(preChecklist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPreChecklist), new { tripId = preChecklist.TripId }, preChecklist);
        }



        [HttpPut("{tripId}")]
        public async Task<IActionResult> UpdatePreChecklist(int tripId, PreChecklist preChecklist)
        {
            if (tripId != preChecklist.TripId)
            {
                return BadRequest();
            }

            _context.Entry(preChecklist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreChecklistExists(tripId))
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

        [HttpDelete("{tripId}")]
        public async Task<IActionResult> DeletePreChecklist(int tripId)
        {
            var preChecklist = await _context.PreChecklists
                .FirstOrDefaultAsync(pc => pc.TripId == tripId);

            if (preChecklist == null)
            {
                return NotFound();
            }

            _context.PreChecklists.Remove(preChecklist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreChecklistExists(int tripId)
        {
            return _context.PreChecklists.Any(e => e.TripId == tripId);
        }
    }
}
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
           // var preChecklist = await _context.PreChecklists
               // .FirstOrDefaultAsync(pc => pc.TripId == tripId);

           // if (preChecklist == null)
            {
                return NotFound();
            }

           // return preChecklist;
        }
        [HttpPost]
        public async Task<ActionResult<PreChecklist>> CreatePreChecklist([FromBody] PreChecklist preChecklist)
        {
            if (preChecklist == null)
            {
                return BadRequest("PreChecklist cannot be null.");
            }

            try
            {
                _context.PreChecklists.Add(preChecklist);
                await _context.SaveChangesAsync();

                // Return status 200 OK with the created entity
                return Ok(preChecklist);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                // Example: _logger.LogError(ex, "An error occurred while saving the PreChecklist.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the PreChecklist.");
            }
            catch (Exception ex)
            {
                // Log the exception details
                // Example: _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }



        // private bool PreChecklistExists(int tripId)
        // {
        //   return _context.PreChecklists.Any(e => e.TripId == tripId);
        // }
    }
}
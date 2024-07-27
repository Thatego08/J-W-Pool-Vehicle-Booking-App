using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefuelVehicleController : ControllerBase
    {
        private readonly IRefuelVehicleRepository _refuelVehicleRepository;

        public RefuelVehicleController(IRefuelVehicleRepository refuelVehicleRepository)
        {
            _refuelVehicleRepository = refuelVehicleRepository;
        }

        // GET: api/RefuelVehicle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefuelVehicle>>> GetRefuelVehicles()
        {
            var refuelVehicles = await _refuelVehicleRepository.GetAllAsync();
            return Ok(refuelVehicles);
        }

        // GET: api/RefuelVehicle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RefuelVehicle>> GetRefuelVehicle(int id)
        {
            var refuelVehicle = await _refuelVehicleRepository.GetByIdAsync(id);
            if (refuelVehicle == null)
            {
                return NotFound();
            }

            return Ok(refuelVehicle);
        }

        [HttpPost]
        public async Task<ActionResult<RefuelVehicle>> PostRefuelVehicle([FromBody] RefuelVehicle refuelVehicle)
        {
            if (refuelVehicle == null)
            {
                return BadRequest("RefuelVehicle object is null.");
            }

            // If TripId is provided, validate that it exists in the database
            if (refuelVehicle.TripId.HasValue)
            {
                var tripExists = await _refuelVehicleRepository.CheckIfTripExists(refuelVehicle.TripId.Value);
                if (!tripExists)
                {
                    return BadRequest("Trip does not exist.");
                }
            }

            await _refuelVehicleRepository.AddAsync(refuelVehicle);
            return CreatedAtAction(nameof(GetRefuelVehicle), new { id = refuelVehicle.RefuelVehicleId }, refuelVehicle);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefuelVehicle(int id, [FromBody] RefuelVehicle refuelVehicle)
        {
            if (refuelVehicle == null)
            {
                return BadRequest("RefuelVehicle object is null.");
            }

            if (id != refuelVehicle.RefuelVehicleId)
            {
                return BadRequest("RefuelVehicle ID mismatch.");
            }

            // Validate TripId if provided
            if (refuelVehicle.TripId.HasValue)
            {
                var tripExists = await _refuelVehicleRepository.CheckIfTripExists(refuelVehicle.TripId.Value);
                if (!tripExists)
                {
                    return BadRequest("Trip does not exist.");
                }
            }

            await _refuelVehicleRepository.UpdateAsync(refuelVehicle);
            return NoContent();
        }


        // DELETE: api/RefuelVehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefuelVehicle(int id)
        {
            await _refuelVehicleRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

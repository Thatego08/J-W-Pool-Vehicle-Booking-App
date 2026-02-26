using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using static Team34FinalAPI.ViewModels.RatesViewModelEE;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesEEController : ControllerBase
    {
        private readonly RateEEDBContext _context;

        public RatesEEController(RateEEDBContext context)
        {
            _context = context;
        }

        // GET: api/RatesEE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatesViewModelEE.RateResponseDto>>> GetRates()
        {
            var rates = await _context.RatesEE
                .Include(r => r.Project)   // eager load project for ProjectNumber & Description
                .ToListAsync();

            return rates.Select(r => new RatesViewModelEE.RateResponseDto
            {
                RateId = r.RateId,
                RateName = r.RateName,
                RateValue = r.RateValue,
                EffectiveDate = r.EffectiveDate,
                ExpiryDate = r.ExpiryDate,
                IsActive = r.IsActive,
                ProjectId = r.ProjectId,
                ProjectNumber = r.Project?.ProjectNumber ?? 0,
                Description = r.Project?.Description,
                CreatedAt = r.CreatedAt,
                CreatedBy = r.CreatedBy,
                UpdatedAt = r.UpdatedAt,
                UpdatedBy = r.UpdatedBy
            }).ToList();
        }

        // GET: api/RatesEE/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RatesViewModelEE.RateResponseDto>> GetRate(int id)
        {
            var rate = await _context.RatesEE
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.RateId == id);

            if (rate == null)
                return NotFound();

            return new RatesViewModelEE.RateResponseDto
            {
                RateId = rate.RateId,
                RateName = rate.RateName,
                RateValue = rate.RateValue,
                EffectiveDate = rate.EffectiveDate,
                ExpiryDate = rate.ExpiryDate,
                IsActive = rate.IsActive,
                ProjectId = rate.ProjectId,
                ProjectNumber = rate.Project?.ProjectNumber ?? 0,
                Description = rate.Project?.Description,
                CreatedAt = rate.CreatedAt,
                CreatedBy = rate.CreatedBy,
                UpdatedAt = rate.UpdatedAt,
                UpdatedBy = rate.UpdatedBy
            };
        }

        // POST: api/RatesEE
        [HttpPost]
        public async Task<ActionResult<RatesViewModelEE.RateResponseDto>> PostRate(RatesViewModelEE.CreateRateDto createDto)
        {
            // Validate project exists
            var project = await _context.Projects.FindAsync(createDto.ProjectId);
            if (project == null)
                return BadRequest($"Project with ID {createDto.ProjectId} does not exist.");

            // Get the logged-in username (from ClaimsPrincipal)
            var userName = User.Identity?.Name ?? "System";

            var rate = new RatesEE
            {
                RateName = createDto.RateName,
                RateValue = createDto.RateValue,
                EffectiveDate = createDto.EffectiveDate,
                ExpiryDate = createDto.ExpiryDate,
                IsActive = createDto.IsActive,
                ProjectId = createDto.ProjectId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName,
                UpdatedAt = null,          // no update yet, so null is fine
                UpdatedBy = userName       // set to same user (or could be null if column nullable)
            };

            _context.RatesEE.Add(rate);
            await _context.SaveChangesAsync();

            // Load project for response
            await _context.Entry(rate).Reference(r => r.Project).LoadAsync();

            var response = new RatesViewModelEE.RateResponseDto
            {
                RateId = rate.RateId,
                RateName = rate.RateName,
                RateValue = rate.RateValue,
                EffectiveDate = rate.EffectiveDate,
                ExpiryDate = rate.ExpiryDate,
                IsActive = rate.IsActive,
                ProjectId = rate.ProjectId,
                ProjectNumber = rate.Project?.ProjectNumber ?? 0,
                Description = rate.Project?.Description,
                CreatedAt = rate.CreatedAt,
                CreatedBy = rate.CreatedBy,
                UpdatedAt = rate.UpdatedAt,
                UpdatedBy = rate.UpdatedBy
            };

            return CreatedAtAction(nameof(GetRate), new { id = rate.RateId }, response);
        }

        // PUT: api/RatesEE/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRate(int id, RatesViewModelEE.UpdateRateDto updateDto)
        {
            if (id != updateDto.RateId)
                return BadRequest("Rate ID mismatch.");

            var rate = await _context.RatesEE.FindAsync(id);
            if (rate == null)
                return NotFound();

            // Get the logged-in username (from ClaimsPrincipal)
            var userName = User.Identity?.Name ?? "System";

            // Optionally validate new ProjectId
            var project = await _context.Projects.FindAsync(updateDto.ProjectId);
            if (project == null)
                return BadRequest($"Project with ID {updateDto.ProjectId} does not exist.");

            rate.RateName = updateDto.RateName;
            rate.RateValue = updateDto.RateValue;
            rate.EffectiveDate = updateDto.EffectiveDate;
            rate.ExpiryDate = updateDto.ExpiryDate;
            rate.IsActive = updateDto.IsActive;
            rate.ProjectId = updateDto.ProjectId;
            rate.UpdatedAt = DateTime.UtcNow;
            rate.UpdatedBy = userName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/RatesEE/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            var rate = await _context.RatesEE.FindAsync(id);
            if (rate == null)
                return NotFound();

            _context.RatesEE.Remove(rate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
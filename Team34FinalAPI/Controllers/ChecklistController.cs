using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly VehicleDbContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(
            VehicleDbContext context,
            IVehicleRepository vehicleRepository,
            IChecklistRepository checklistRepository,
            ILogger<ChecklistController> logger)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _checklistRepository = checklistRepository;
            _logger = logger;
        }

        [HttpGet("GetAllChecklists")]
        public async Task<IActionResult> GetAllChecklists()
        {
            try
            {
                var results = await _context.VehicleChecklists
                    .Include(c => c.Vehicle) // Assuming Vehicle is a navigation property
                    .ToListAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicle checklists.");
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle checklists.");
            }
        }

        [HttpPost("AddVehicleChecklist")]
        public async Task<IActionResult> AddVehicleChecklist([FromBody] VehicleChecklist checklist)
        {
            if (checklist == null)
            {
                return BadRequest("Checklist cannot be null.");
            }

            try
            {
                await _checklistRepository.AddChecklistAsync(checklist);
                return Ok(checklist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the vehicle checklist.");
                return StatusCode(500, "Internal Server Error: Unable to add vehicle checklist.");
            }
        }

        [HttpPost("AddPostChecklist")]
        public async Task<IActionResult> AddPostChecklist([FromBody] PostChecklist checklist)
        {
            if (checklist == null)
            {
                return BadRequest("Checklist cannot be null.");
            }

            if (!checklist.ReturnVehicle)
            {
                return BadRequest("ReturnVehicle must be checked to submit the PostChecklist.");
            }

            try
            {
                // Get the last checklist for the vehicle
                var lastChecklist = await _checklistRepository.GetLastVehicleChecklistAsync(checklist.VehicleId);
                if (lastChecklist != null)
                {
                    checklist.OpeningKms = lastChecklist.ClosingKms;
                }

                await _checklistRepository.AddPostChecklistAsync(checklist);
                return Ok(checklist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the post checklist.");
                return StatusCode(500, "Internal Server Error: Unable to add post checklist.");
            }
        }

        [HttpGet("GetVehicleChecklists")]
        public async Task<IActionResult> GetVehicleChecklists()
        {
            try
            {
                var checklists = await _context.VehicleChecklists.ToListAsync();
                return Ok(checklists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicle checklists.");
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle checklists.");
            }
        }

        [HttpGet("GetPostChecklists")]
        public async Task<IActionResult> GetPostChecklists()
        {
            try
            {
                var checklists = await _context.PostChecklist.ToListAsync();
                return Ok(checklists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving post checklists.");
                return StatusCode(500, "Internal Server Error: Unable to retrieve post checklists.");
            }
        }
    }
}





/*
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly VehicleDbContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(VehicleDbContext context, IVehicleRepository vehicleRepository, ILogger<ChecklistController> logger)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        [HttpGet("GetAllChecklists")]
        public async Task<IActionResult> GetAllChecklists()
        {
            try
            {
                var results = await _context.VehicleChecklists
                    .Include(c => c.VehicleId)
                    .Include(c => c.ExteriorChecks)
                    .Include(c => c.InteriorChecks)
                    .Include(c => c.UnderTheHoodChecks)
                    .Include(c => c.FunctionalTests)
                    .Include(c => c.SafetyEquipment)
                    .Include(c => c.Documentation)
                    .ToListAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicle checklists.");
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle checklists.");
            }
        }


        [HttpPost("AddVehicleChecklist")]
        public async Task<IActionResult> AddVehicleChecklist(VehicleChecklist checklist)
        {
            await _checklistRepository.AddChecklistAsync(checklist);
            await _checklistRepository.SaveChangesAsync();
            return Ok(checklist);
        }

        [HttpPost("AddPostChecklist")]
        public async Task<IActionResult> AddPostChecklist(PostChecklist checklist)
        {
            if (!checklist.ReturnVehicle)
            {
                return BadRequest("ReturnVehicle must be checked to submit the PostChecklist.");
            }

            // Set OpeningKms to the last ClosingKms for the given vehicle
            var lastChecklist = await _checklistRepository.GetLastVehicleChecklistAsync(checklist.VehicleId);
            if (lastChecklist != null)
            {
                checklist.OpeningKms = lastChecklist.ClosingKms;
            }

            await _checklistRepository.AddPostChecklistAsync(checklist);
            await _checklistRepository.SaveChangesAsync();
            return Ok(checklist);
        }

        [HttpGet("GetVehicleChecklists")]
        public async Task<IActionResult> GetVehicleChecklists()
        {
            var checklists = await _context.VehicleChecklists.ToListAsync();  // This can also be moved to the repository
            return Ok(checklists);
        }

        [HttpGet("GetPostChecklists")]
        public async Task<IActionResult> GetPostChecklists()
        {
            var checklists = await _context.PostChecklist.ToListAsync();  // This can also be moved to the repository
            return Ok(checklists);
        }
    }

}

        
        [HttpPost("SubmitChecklist")]
        public async Task<IActionResult> SubmitChecklist([FromBody] PostChecklist checklist)
        {
            if (checklist == null)
            {
                return BadRequest("Checklist is required.");
            }

            if (!checklist.ReturnVehicle)
            {
                return BadRequest("You must return the vehicle before submitting.");
            }

            try
            {
                var existingChecklist = await _context.PostChecklist
                    .Where(c => c.VehicleId == checklist.VehicleId)
                    .OrderByDescending(c => c.PostId)
                    .FirstOrDefaultAsync();

                if (existingChecklist != null)
                {
                    checklist.OpeningKms = existingChecklist.ClosingKms;
                }

                _context.PostChecklist.Add(checklist);
                await _context.SaveChangesAsync();

                return Ok(checklist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the post checklist.");
                return StatusCode(500, "Internal Server Error: Unable to submit checklist.");
            }
        }

        [HttpPut("UpdateChecklist")]
        public async Task<IActionResult> UpdateChecklist([FromBody] VehicleChecklist checklist)
        {
            if (checklist == null)
            {
                return BadRequest("Checklist is required.");
            }

            try
            {
                var existingChecklist = await _context.VehicleChecklists.FindAsync(checklist.Id);

                if (existingChecklist == null)
                {
                    return NotFound("Checklist not found.");
                }

                _context.Entry(existingChecklist).CurrentValues.SetValues(checklist);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Checklist updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the checklist.");
                return StatusCode(500, "Internal Server Error: Unable to update checklist.");
            }
        }
    }
}*/


/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly VehicleDbContext _context;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(IWebHostEnvironment environment, IVehicleRepository vehicleRepository, ILogger<ChecklistController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet("GetAllChecklists")]
        public async Task<IActionResult> GetAllVehicleChecklist()
        {
            try
            {
                var results = await _vehicleRepository.GetChecklistsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vehicle checklists.");
                return StatusCode(500, "Internal Server Error: Unable to retrieve vehicle checklists.");
            }
        }

        [HttpPost("SubmitChecklist")]
        public async Task<IActionResult> SubmitChecklist([FromBody] VehicleChecklist checklist)
        {
            if (!checklist.ReturnVehicle)
            {
                return BadRequest("You must return the vehicle before submitting.");
            }

            var existingChecklist = await _context.VehicleChecklists
                .Where(c => c.VehicleId == checklist.VehicleId)
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            if (existingChecklist != null)
            {
                checklist.OpeningKms = existingChecklist.ClosingKms;
            }

            _context.VehicleChecklists.Add(checklist);
            await _context.SaveChangesAsync();

            return Ok(checklist);
        }

        [HttpPut("UpdateChecklist")]
        public async Task<IActionResult> UpdateChecklist([FromBody] VehicleChecklist checklist)
        {
            if (checklist == null) return BadRequest("Checklist is required.");

            try
            {
                await _vehicleRepository.UpdateChecklistAsync(checklist);
                return Ok(new { message = "Checklist updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the checklist.");
                return StatusCode(500, "Internal Server Error: Unable to update checklist.");
            }
        }
    }
} */

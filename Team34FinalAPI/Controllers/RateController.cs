using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateController : ControllerBase
    {
        private readonly IRateRepo _rateRepo;
        private readonly IAuditLogRepository _auditLogRepo;


        public RateController(IRateRepo rateRepo, IAuditLogRepository auditLogRepo)
        {
            _rateRepo = rateRepo;
            _auditLogRepo = auditLogRepo;
        }


        [HttpGet]
        [Route(("get-rate"))]
        public async Task<IActionResult> GetAllRates()
        {
            var rates = await _rateRepo.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpPost]
        [Route(("create-rate"))]
        public async Task<IActionResult> CreateRate([FromBody] Rate rate)
        {
            if (rate == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRate = await _rateRepo.CreateRateAsync(rate);


            //Audit Log stuff 
            await _auditLogRepo.AddLogAsync(new AuditLog
            {
                Action = "Create Rate",
                Details = $"Rate for projects has been created by an administrator",
                Timestamp = DateTime.UtcNow
            });

            // Instead of CreatedAtAction, use a simpler response
            return Ok(new { message = "Rate created successfully", rate = createdRate });
        }

        [HttpPut]
        [Route("update-rate")]
        public async Task<IActionResult> UpdateRate(int id, [FromBody] Rate rate)
        {
            if (rate == null || rate.RateID != id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rateToUpdate = await _rateRepo.GetRateByIdAsync(id);
            if (rateToUpdate == null)
            {
                return NotFound();
            }

            await _rateRepo.UpdateRateAsync(rate);

            //Audit Log stuff 
            await _auditLogRepo.AddLogAsync(new AuditLog
            {
                Action = "Update Rate",
                Details = $"Rate for projects has been updated by an administrator",
                Timestamp = DateTime.UtcNow
            });
            return NoContent();
        }
/*
        [HttpGet]
        [Route("get-rates-with-details")]
        public async Task<IActionResult> GetAllRatesWithDetails()
        {
            var rates = await _rateRepo.GetAllRatesWithDetailsAsync();
            return Ok(rates);
        }*/


    }
}

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


        public RateController(IRateRepo rateRepo)
        {
            _rateRepo = rateRepo;

        }


        [HttpGet]
        public async Task<IActionResult> GetAllRates()
        {
            var rates = await _rateRepo.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRate([FromBody] Rate rate)
        {
            if (rate == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRate = await _rateRepo.CreateRateAsync(rate);

            // Instead of CreatedAtAction, use a simpler response
            return Ok(new { message = "Rate created successfully", rate = createdRate });
        }

        [HttpPut("{id}")]
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
            return NoContent();
        }

   
    }
    }

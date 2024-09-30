using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Team34FinalAPI.ViewModels;
using Team34FinalAPI.Data;
using Microsoft.EntityFrameworkCore;


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

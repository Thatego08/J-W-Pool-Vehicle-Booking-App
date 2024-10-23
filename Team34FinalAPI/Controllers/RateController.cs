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

            // Existing Get method remains unchanged
            [HttpGet]
            [Route("get-rate")]
            public async Task<IActionResult> GetAllRates()
            {
                var rates = await _rateRepo.GetAllRatesAsync();
                return Ok(rates);
            }

            // Create method adjusted to use RateViewModel
            [HttpPost]
            [Route("create-rate")]
            public async Task<IActionResult> CreateRate([FromBody] RateViewModel rateViewModel)
            {
                if (rateViewModel == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Convert ViewModel to actual Rate entity
                var newRate = new Rate
                {
                    ProjectID = rateViewModel.ProjectID,
                    RateValue = rateViewModel.RateValue,
                    ApplicableTimePeriod = rateViewModel.ApplicableTimePeriod,
                    Conditions = rateViewModel.Conditions
                };

                var createdRate = await _rateRepo.CreateRateAsync(newRate);

                return Ok(new { message = "Rate created successfully", rate = createdRate });
            }

            // Updated method to use RateViewModel
            [HttpPut]
            [Route("update-rate/{id}")]
            public async Task<IActionResult> UpdateRate(int id, [FromBody] RateViewModel rateViewModel)
            {
                if (rateViewModel == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the existing rate entity by ID
                var rateToUpdate = await _rateRepo.GetRateByIdAsync(id);
                if (rateToUpdate == null)
                {
                    return NotFound();
                }

                // Now map the RateViewModel to the Rate entity
                var rateEntity = new Rate
                {
                    RateID = id, // Ensure the ID matches the rate being updated
                    ProjectID = rateViewModel.ProjectID,
                    RateValue = rateViewModel.RateValue,
                    ApplicableTimePeriod = rateViewModel.ApplicableTimePeriod,
                    Conditions = rateViewModel.Conditions
                };

                // Now pass the rate entity to UpdateRateAsync
                await _rateRepo.UpdateRateAsync(rateEntity);

                return NoContent();
            }


        }


    }


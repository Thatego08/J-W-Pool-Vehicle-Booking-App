using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    public class RateController : Controller
    {

        private readonly IRateRepo _rateRepo;

        public RateController(IRateRepo rateRepo)
        {
            _rateRepo = rateRepo;
        }

        [HttpPost]
        [Route("createRate")]
        public async Task<IActionResult> CreateRate([FromBody] Rate rate)
        {
            if (rate == null || string.IsNullOrEmpty(rate.RateType) || rate.ProjectId <= 0)
            {
                return BadRequest("Invalid rate data.");
            }

            try
            {
                var created = await _rateRepo.CreateRate(rate);
                if (!created)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return Ok("Rate created successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("updateRate")]
        public async Task<IActionResult> UpdateRate([FromBody] Rate rate)
        {
            if (rate == null || string.IsNullOrEmpty(rate.RateType) || rate.ProjectId <= 0)
            {
                return BadRequest("Invalid rate data.");
            }

            try
            {
                var updated = await _rateRepo.UpdateRate(rate);
                if (!updated)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return Ok("Rate updated successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }


}

using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models.CreateDto;
using Team34FinalAPI.Models.UpdateDto;
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
        private readonly IProjectRepository _projectRepository;
        private readonly IConfiguration _configuration;

        public RateController(IProjectRepository projectRepository, IRateRepo rateRepo, IConfiguration configuration)
        {
            _rateRepo = rateRepo;
            _projectRepository = projectRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("createRate")]
        public async Task<IActionResult> CreateRate([FromBody] CreateRateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //try
            //{
            //    var project = _rateRepo.Project.FirstOrDefault(p => p.ProjectNumber == dto.ProjectNumber);
            //    if (project == null)
            //    {
            //        return NotFound("Project not found");
            //    }

            //    var rateType = _rateRepo.RateTypes.FirstOrDefault(rt => rt.TypeName == dto.TypeName);
            //    if (rateType == null)
            //    {
            //        return NotFound("RateType not found");
            //    }

            //    var rate = new Rate
            //    {
            //        ProjectID = project.ProjectID,
            //        RateTypeID = rateType.RateTypeID,
            //        RateValue = dto.RateValue,
            //        ApplicableTimePeriod = dto.ApplicableTimePeriod,
            //        Conditions = dto.Conditions
            //    };

            //    var created = await _rateRepo.CreateRate(rate);
            //    if (!created)
            //    {
            //        return BadRequest("Failed to create rate");
            //    }
            //    return Ok("Successfully Created Rate");
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception
            //    Console.WriteLine($"Error in CreateRate: {ex.Message}");
               return BadRequest("An Error Occurred, Please Try Again Later");
            //}
        }

        [HttpPost("AddRate")]
        public async Task<IActionResult> CreateRate(RateViewModel rateVM)
        {
            var rate = new Rate
            {
                RateTypeID = rateVM.RateTypeID,
                RateValue = rateVM.RateValue,
                ProjectID = rateVM.ProjectID,
                ApplicableTimePeriod = rateVM.ApplicableTimePeriod,
                Conditions = rateVM.Conditions
            };

            var createdRate = await _rateRepo.AddRateAsync(rate);

            return CreatedAtAction(nameof(CreateRate), new { id = createdRate.RateID }, createdRate);
        }

        [HttpPost]
        [Route("updateRate/{id}")]
        public async Task<IActionResult> UpdateRate([FromBody] UpdateRateDto dto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rate = await _rateRepo.GetRate(id);
                if (rate == null)
                {
                    return NotFound("Rate not found");
                }

                rate.RateValue = dto.RateValue;
                rate.ApplicableTimePeriod = dto.ApplicableTimePeriod;
                rate.Conditions = dto.Conditions;

                var updated = await _rateRepo.UpdateRate(rate);
                if (!updated)
                {
                    return BadRequest("Failed to update rate");
                }
                return Ok("Successfully Updated Rate");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in UpdateRate: {ex.Message}");
                return BadRequest("An Error Occurred, Please Try Again Later");
            }
        }
    }
}

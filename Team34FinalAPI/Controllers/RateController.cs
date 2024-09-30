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
        private readonly IProjectRepository _projectRepository; // Injecting project repository
        private readonly AppDbContext _context; // Injecting DbContext for RateType access



        public RateController(IRateRepo rateRepo, IProjectRepository projectRepository, AppDbContext context)
        {
            _rateRepo = rateRepo;
            _projectRepository = projectRepository; // Initialize _projectRepository
            _context = context;

        }


        [HttpGet]
        [Route(("get-rate"))]
        public async Task<IActionResult> GetAllRates()
        {
            var rates = await _rateRepo.GetAllRatesAsync();
            return Ok(rates);
        }

        [HttpPost]
        [Route("create-rate")]
        public async Task<IActionResult> CreateRate([FromBody] RateViewModel rateViewModel)
        {
            if (rateViewModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rateType = await _context.RateTypes.FirstOrDefaultAsync(rt => rt.RateTypeID == rateViewModel.RateTypeID);
            if (rateType == null)
            {
                return BadRequest("RateType not found.");
            }
            // Find the related RateType by RateTypeName (or ID)

            var rate = new Rate
            {
                RateValue = rateViewModel.RateValue,
                RateTypeID = rateType.RateTypeID,  // Set the RateTypeID instead of RateTypeName
                ApplicableTimePeriod = rateViewModel.ApplicableTimePeriod,
                Conditions = rateViewModel.Conditions
            };

            // Handle the many-to-many association between Rate and Projects
            foreach (var projectNumber in rateViewModel.ProjectNumbers)
            {
                var project = await _projectRepository.GetProjectByNumberAsync(projectNumber);
                if (project != null)
                {
                    rate.ProjectRates.Add(new ProjectRate
                    {
                        ProjectID = project.ProjectID,
                        RateID = rate.RateID
                    });
                }
            }

            var createdRate = await _rateRepo.CreateRateAsync(rate);
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        [Route("GetAllStatuses")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _statusRepository.GetAllStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet]
        [Route("GetStatus/{statusId}")]
        public async Task<IActionResult> GetStatus(int statusId)
        {
            var status = await _statusRepository.GetStatusByIdAsync(statusId);
            if (status == null)
            {
                return NotFound("Status not found");
            }

            return Ok(status);
        }
    }
}

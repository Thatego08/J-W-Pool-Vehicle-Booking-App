using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata.Ecma335;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        public DriverController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        [HttpGet]
        [Route("GetAllDrivers")] //Displays All The Drivers Stored On The Database

        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var results = await _driverRepository.GetAllDriverAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please Contact Support");
            }
        }
    }
}
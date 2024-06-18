using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        

        [HttpGet]
        [Route("GetAllServices")] // Return list of vehicles
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var results = await _serviceRepository.GetAllServicesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("GetService/{serviceId}")] // Corrected route template
        public async Task<IActionResult> GetServiceAsync(int serviceId)
        {
            try
            {
                var results = await _serviceRepository.GetServiceAsync(serviceId);

                if (results == null) return NotFound("Service does not exist");
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("AddService")]
        public async Task<IActionResult> AddService(ServiceViewModel svm)
        {
            var service = new Service
            {
                ServiceID = svm.VehicleServiceID,
                VehicleID = svm.VehicleID,
                ServiceDate = svm.ServiceDate,
                 Description = svm.Description,


            };

            try
            {
                _serviceRepository.Add(service);
                await _serviceRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(service);
        }

       
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly VehicleDbContext _context;

        public ServiceController(VehicleDbContext context)
        {
            _context = context;
        }

        // GET: api/service
        [HttpGet("GetServices")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllServices()
        {
            var services = await _context.VehicleService.ToListAsync();
            var serviceDtos = services.Select(service => new ServiceDto
            {
                ServiceID = service.ServiceID,
                VehicleID = service.VehicleID,
                AdminName = service.AdminName,
                AdminEmail = service.AdminEmail,
                VehicleModelName = service.VehicleModelName,
                VehicleMakeName = service.VehicleMakeName,
                Description = service.Description,
                ServiceDate = service.ServiceDate
            }).ToList();

            return Ok(serviceDtos);
        }

        // GET: api/service/{id}
        [HttpGet("GetService/{serviceId}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int id)
        {
            var service = await _context.VehicleService.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var serviceDto = new ServiceDto
            {
                ServiceID = service.ServiceID,
                VehicleID = service.VehicleID,
                AdminName = service.AdminName,
                AdminEmail = service.AdminEmail,
                VehicleModelName = service.VehicleModelName,
                VehicleMakeName = service.VehicleMakeName,
                Description = service.Description,
                ServiceDate = service.ServiceDate
            };

            return Ok(serviceDto);
        }

        // POST: api/service
        [HttpPost("CreateService")]
        public async Task<ActionResult<ServiceDto>> CreateService(ServiceDto serviceDto)
        {
            var service = new Service
            {
                VehicleID = serviceDto.VehicleID,
                AdminName = serviceDto.AdminName,
                AdminEmail = serviceDto.AdminEmail,
                VehicleModelName = serviceDto.VehicleModelName,
                VehicleMakeName = serviceDto.VehicleMakeName,
                Description = serviceDto.Description,
                ServiceDate = serviceDto.ServiceDate
            };

            _context.VehicleService.Add(service);
            await _context.SaveChangesAsync();

            serviceDto.ServiceID = service.ServiceID;

            return CreatedAtAction(nameof(GetServiceById), new { id = service.ServiceID }, serviceDto);
        }


    }
}

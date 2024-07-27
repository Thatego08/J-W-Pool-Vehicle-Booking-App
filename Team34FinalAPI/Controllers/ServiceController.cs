using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            var services = await _context.Service.ToListAsync();
            var serviceDtos = services.Select(service => new ServiceDto
            {
                ServiceID = service.ServiceID,
                VehicleID = service.VehicleID,
                AdminName = service.AdminName,
                AdminEmail = service.AdminEmail,
                Description = service.Description,
                ServiceDate = service.ServiceDate
            }).ToList();

            return Ok(serviceDtos);
        }

        // GET: api/service/{id}
        [HttpGet("GetService/{serviceId}")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int serviceId)
        {
            var service = await _context.Service.FindAsync(serviceId);
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
                Description = serviceDto.Description,
                ServiceDate = serviceDto.ServiceDate
            };

            _context.Service.Add(service);
            await _context.SaveChangesAsync();

            serviceDto.ServiceID = service.ServiceID;

            return CreatedAtAction(nameof(GetServiceById), new { serviceId = service.ServiceID }, serviceDto);
        }

        // PUT: api/service/{id}
        [HttpPut("UpdateService/{id}")]
        public async Task<IActionResult> UpdateService(int id, ServiceDto serviceDto)
        {
            if (id != serviceDto.ServiceID)
            {
                return BadRequest("Service ID mismatch");
            }

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            service.VehicleID = serviceDto.VehicleID;
            service.AdminName = serviceDto.AdminName;
            service.AdminEmail = serviceDto.AdminEmail;
            service.Description = serviceDto.Description;
            service.ServiceDate = serviceDto.ServiceDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ServiceID == id);
        }
    }
}

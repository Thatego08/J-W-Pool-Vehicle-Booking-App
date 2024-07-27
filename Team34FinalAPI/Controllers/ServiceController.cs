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
        /*
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
        */

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

            // Send email notification
            try
            {
                await SendEmail(serviceDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // Consider whether you want to return an error response if the email fails
                // e.g., return StatusCode(StatusCodes.Status500InternalServerError, "Error sending email");
            }

            return CreatedAtAction(nameof(GetServiceById), new { serviceId = service.ServiceID }, serviceDto);
        }



        // PUT: api/service/{id}
        [HttpPut("UpdateService/{id}")]
        public async Task<IActionResult> UpdateService(int id, ServiceDto serviceDto)
        {
            // Check if the provided ID matches the ServiceDto ID
            if (id != serviceDto.ServiceID)
            {
                return BadRequest("Service ID mismatch");
            }

            // Fetch the existing service record from the database
            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            // Update the service record with the new values
            service.VehicleID = serviceDto.VehicleID;
            service.AdminName = serviceDto.AdminName;
            service.AdminEmail = serviceDto.AdminEmail;
            service.Description = serviceDto.Description;
            service.ServiceDate = serviceDto.ServiceDate;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues if needed
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

        // Helper method to check if a service exists
        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ServiceID == id);
        }

        private async Task SendEmail(ServiceDto serviceDto)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("Your App Name", "yourapp@example.com"));
                message.To.Add(new MimeKit.MailboxAddress(serviceDto.AdminName, serviceDto.AdminEmail));
                message.Subject = "Service Booking Confirmation";

                message.Body = new MimeKit.TextPart("plain")
                {
                    Text = $"Dear {serviceDto.AdminName},\n\n" +
                           $"Your service booking has been confirmed.\n\n" +
                           $"Vehicle Registration Number: {serviceDto.VehicleID}\n" +
                           $"Service Date: {serviceDto.ServiceDate.ToShortDateString()}\n\n" +
                           $"Thank you."
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, false);
                    await client.AuthenticateAsync("your_smtp_username", "your_smtp_password");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                // Log successful email sending
                Console.WriteLine($"Email sent to {serviceDto.AdminEmail}");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }


        /*
        private async Task SendEmail(ServiceDto serviceDto)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("Your App Name", "yourapp@example.com"));
            message.To.Add(new MimeKit.MailboxAddress(serviceDto.AdminName, serviceDto.AdminEmail));
            message.Subject = "Service Booking Confirmation";

            message.Body = new MimeKit.TextPart("plain")
            {
                Text = $"Dear {serviceDto.AdminName},\n\n" +
                       $"Your service booking has been confirmed.\n\n" +
                       $"Vehicle Registration Number: {serviceDto.VehicleID}\n" +
                       $"Service Date: {serviceDto.ServiceDate.ToShortDateString()}\n\n" +
                       $"Thank you."
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.example.com", 587, false);
                await client.AuthenticateAsync("your_smtp_username", "your_smtp_password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        */

    }
}

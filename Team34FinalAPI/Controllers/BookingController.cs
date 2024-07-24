using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team34FinalAPI.Models;
using Team34FinalAPI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Team34FinalAPI.Services;

namespace Team34FinalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingDbContext _context;
        private readonly VehicleDbContext _vehicleContext;

        public BookingController(IBookingRepository bookingRepository, BookingDbContext context, VehicleDbContext vehicleContext)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _vehicleContext = vehicleContext;
        }

        // Get: api/booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> GetBookingsAsync()
        {
            // Returns a list of all Bookings including related Vehicle and Project data
            var bookings = await _bookingRepository.GetBookingsAsync();

            // Map Booking model to BookingViewModel
            var bookingViewModels = MapToViewModel(bookings);

            return Ok(bookingViewModels);
        }

        // Get specific Booking
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingViewModel>> GetBookingAsync(int id)
        {
            // Find specific booking by Id including related Vehicle and Project data
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            // Return 404 error if Booking not found
            if (booking == null) return NotFound();
            // Map Booking model to BookingViewModel
            var bookingViewModel = MapToViewModel(booking);
            return Ok(bookingViewModel);
        }

        // Adds a Booking to Db
        [HttpPost]
        public async Task<ActionResult<BookingViewModel>> PostBookingAsync(BookingViewModel bookingViewModel)
        {
            // Fetch the vehicle based on the provided vehicle name
            var vehicle = await _vehicleContext.Vehicles.SingleOrDefaultAsync(v => v.Name == bookingViewModel.VehicleName);
            if (vehicle == null) return BadRequest($"Vehicle with name {bookingViewModel.VehicleName} does not exist.");

            // Fetch the project based on the provided project name
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.ProjectNumber == bookingViewModel.ProjectNumber);
            if (project == null) return BadRequest($"Project with name {bookingViewModel.ProjectNumber} does not exist.");

            // Map BookingViewModel to Booking model
            var booking = new Booking
            {
                UserName = bookingViewModel.UserName,
                Event = bookingViewModel.Event,
                StartDate = bookingViewModel.StartDate,
                EndDate = bookingViewModel.EndDate,
                VehicleId = vehicle.VehicleID,
                ProjectId = project.ProjectID
            };

            // Add new Booking to context
            await _bookingRepository.AddBookingAsync(booking);

            // Map back to BookingViewModel for the response
            bookingViewModel.BookingID = booking.BookingID;


            // Return 201 created response with newly created booking
            return CreatedAtAction(nameof(GetBookingAsync), new { id = booking.BookingID }, bookingViewModel);
        }



        // Edit booking
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingAsync(int id, BookingViewModel bookingViewModel)
        {
            // If booking Id in url does not match booking id in request body, return Bad request response 400
            if (id != bookingViewModel.BookingID) return BadRequest();


            // Find the existing booking
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();


            // Update booking properties
            booking.UserName = bookingViewModel.UserName;
            booking.Event = bookingViewModel.Event;
            booking.StartDate = bookingViewModel.StartDate;
            booking.EndDate = bookingViewModel.EndDate;

            try
            {
                await _bookingRepository.UpdateBookingAsync(booking);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Return 404 Not found response if booking does not exist
                if (!BookingExists(id)) return NotFound();
                throw;

            }

            // Return 204 No Content response indicating that the update was successful
            return NoContent();
        }

        // Delete Booking
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingAsync(int id)
        {
            // Find booking to be deleted by Id
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();


            // Remove booking from context
            await _bookingRepository.DeleteBookingAsync(id);

            // Return 204 No Content response indicating that the deletion was successful
            return NoContent();
        }



        //Search booking history
        [HttpGet("history/{vehicleId}")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> GetBookingHistoryAsync(int vehicleId)
        {
            var bookings = await _bookingRepository.GetBookingsByVehicleIdAsync(vehicleId);

            var bookingViewModels = MapToViewModel(bookings);

            return Ok(bookingViewModels);
        }

        // Helper method to check if a booking exists by Id
        private bool BookingExists(int id)
        {
            return _bookingRepository.BookingExists(id);
        }

        private List<BookingViewModel> MapToViewModel(IEnumerable<Booking> bookings)
        {
            return bookings.Select(b => new BookingViewModel
            {
                BookingID = b.BookingID,
                UserName = b.UserName,
                Event = b.Event,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                VehicleName = b.Vehicle.Name,
                ProjectNumber = b.Project?.ProjectNumber
            }).ToList();
        }

        private BookingViewModel MapToViewModel(Booking booking)
        {
            return new BookingViewModel
            {
                BookingID = booking.BookingID,
                UserName = booking.UserName,
                Event = booking.Event,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                VehicleName = booking.Vehicle.Name,
                ProjectNumber = booking.Project?.ProjectNumber
            };
        }
    }
}
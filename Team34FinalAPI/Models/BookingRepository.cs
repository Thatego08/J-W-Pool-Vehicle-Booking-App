using Microsoft.EntityFrameworkCore;

namespace Team34FinalAPI.Models
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;

        public BookingRepository(BookingDbContext context)
        {
            _context = context;
        }


        public async Task<Booking> GetConflictingBookingAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings
                .Where(b => b.VehicleId == vehicleId &&
                            ((b.StartDate <= endDate && b.StartDate >= startDate) || // Start date within range
                             (b.EndDate >= startDate && b.EndDate <= endDate)))      // End date within range
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            try
            {
                var bookings = await _context.Bookings
                                             .Include(b => b.Vehicle)
                                             .Include(b => b.Project)
                                             .ToListAsync();

                foreach (var booking in bookings)
                {
                    Console.WriteLine($"BookingID: {booking.BookingID}");
                }

                return bookings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookingsAsync: {ex.Message}");
                throw;
            }
        }


        public async Task<List<Booking>> GetBookingsWithinNext24HoursAsync()
        {
            var now = DateTime.UtcNow;
            var twentyFourHoursLater = now.AddHours(24);

            return await _context.Bookings
                .Where(b => b.StartDate >= now && b.StartDate <= twentyFourHoursLater && b.StatusId != 4) // Exclude cancelled bookings
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Vehicle)
                    .Include(b => b.Project)
                    .FirstOrDefaultAsync(b => b.BookingID == id);

                Console.WriteLine($"BookingID: {booking.BookingID}");

                return booking;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookingByIdAsync: {ex.Message}");
                throw;
            }
        }


        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingID == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserNameAsync(string username)
        {
            var lowerUsername = username.ToLower(); // Convert username to lowercase

            // Ensure the context has a longer timeout
            _context.Database.SetCommandTimeout(180); // Set timeout to 3 minutes

            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Vehicle)
                .Include(b => b.Project)
                .Where(b => b.UserName.ToLower() == lowerUsername) // Compare using lowercase
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(DateTime fromTime)
        {
            return await _context.Bookings
                .Include(b => b.Vehicle)
                .Where(b => b.StartDate <= fromTime && !b.ReminderSent)
                .ToListAsync();
        }

        

    }
}
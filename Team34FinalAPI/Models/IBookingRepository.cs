namespace Team34FinalAPI.Models
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
        bool BookingExists(int id);
        Task<IEnumerable<Booking>> GetBookingsByVehicleIdAsync(int vehicleId);
    }
}

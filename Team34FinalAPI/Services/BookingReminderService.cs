using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Team34FinalAPI.Models;

namespace Team34FinalAPI.Services
{
    public class BookingReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BookingReminderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendBookingRemindersAsync();
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Run every 10 minutes
            }
        }

        private async Task SendBookingRemindersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var bookings = await bookingRepository.GetBookingsAsync();

                var now = DateTime.Now;
                var pendingCheckins = bookings.Where(b => (b.StartDate - now).TotalMinutes <= 30 && (b.StartDate - now).TotalMinutes > 0).ToList();

                foreach (var booking in pendingCheckins)
                {
                    var message = $"Dear {booking.UserName},\n\nThis is a reminder that you have a booking for the vehicle {booking.Vehicle.Name} starting at {booking.StartDate}.\n\nBest regards,\nTeam";
                    await emailService.SendEmailAsync(booking.UserName, "Booking Reminder", message);
                }
            }
        }
    }
}
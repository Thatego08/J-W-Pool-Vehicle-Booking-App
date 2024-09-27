using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Team34FinalAPI.Models;
using User = Team34FinalAPI.Models.User;

namespace Team34FinalAPI.Services
{
    public class BookingReminderService 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingReminderService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30); // Check every 30 minutes
        private readonly IConfiguration _configuration;

        public BookingReminderService(IServiceProvider serviceProvider,ILogger<BookingReminderService> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;

            // Read the reminder time from configuration
            var reminderTimeInHours = configuration.GetValue<int>("BookingReminder:ReminderTimeInHours");
            _checkInterval = TimeSpan.FromHours(reminderTimeInHours); // Adjust this based on your logic
            
        }

   

        public async Task SendBookingRemindersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var bookings = await bookingRepository.GetBookingsAsync();

                var upcomingBookings = await bookingRepository.GetBookingsWithinNext24HoursAsync();


                var now = DateTime.Now;
                
                foreach (var booking in upcomingBookings)
                {
                    var user = await userManager.FindByNameAsync(booking.UserName);
                    if (user != null)
                    {
                        string subject = "Booking Reminder";
                        string message = $"Dear {user.Name},\n\nThis is a reminder for your booking scheduled on {booking.StartDate} for vehicle {booking.VehicleId}.\n\nThank you!";
                     
                        
                        await emailService.SendEmailAsync(user.Email, subject, message);
                        _logger.LogInformation($"Sent reminder email to {user.Email} for booking ID {booking.BookingID}.");
                    }
                }
            }
        }
    }
}
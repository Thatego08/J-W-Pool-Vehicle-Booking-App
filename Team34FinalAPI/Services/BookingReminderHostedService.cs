using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Team34FinalAPI.Services
{
    public class BookingReminderHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
       //private readonly BookingReminderService _bookingReminderService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingReminderHostedService> _logger;



        public BookingReminderHostedService( IServiceProvider serviceProvider, ILogger<BookingReminderHostedService> logger)
        {
            //_bookingReminderService = bookingReminderService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BookingReminderHostedService starting...");

            // Run every hour, or adjust the interval as needed
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookingReminderService = scope.ServiceProvider.GetRequiredService<BookingReminderService>();

                // Execute the booking reminder task
                bookingReminderService.SendBookingRemindersAsync().GetAwaiter().GetResult();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("BookingReminderHostedService stopping...");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}

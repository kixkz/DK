using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.BL.BackgroundServices
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly Timer _timer;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void DoWork(object? state)
        {
            Thread.Sleep(500);
            _logger.LogInformation($"Timed Hosted Service is working.{DateTime.Now}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1000));
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation($"Hello from {nameof(MyBackgroundService)}");
            }
        }
    }
}

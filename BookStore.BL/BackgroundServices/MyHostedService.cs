using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.BL.BackgroundServices
{
    public class MyHostedService : IHostedService
    {
        private readonly ILogger<MyHostedService> _logger;
        private readonly Timer _timer;

        public MyHostedService(ILogger<MyHostedService> logger)
        {
            _logger = logger;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void DoWork(object? state)
        {
            Thread.Sleep(500);
            _logger.LogInformation($"Timed Hosted Service is working.{DateTime.Now}");
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service stopped.");

            return Task.CompletedTask;
        }
    }
}

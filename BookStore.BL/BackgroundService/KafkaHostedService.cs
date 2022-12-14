using BookStore.BL.Kafka;
using BookStore.Models.Models;
using Microsoft.Extensions.Hosting;

namespace BookStore.BL.BackgroundService
{
    public class KafkaHostedService : IHostedService
    {
        private readonly Consumer<int, Book> _consumer;

        public KafkaHostedService(Consumer<int, Book> consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Consume();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

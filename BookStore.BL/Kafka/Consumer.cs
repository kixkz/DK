using BookStore.Models.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BookStore.BL.Kafka
{
    public class Consumer<TKey, TValue>
    {
        private readonly IOptionsMonitor<KafkaConsumerSettings> _kafkaSettings;
        private readonly IConsumer<TKey, TValue> _consumer;
        public readonly List<TValue> _data;

        public Consumer(IOptionsMonitor<KafkaConsumerSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;

            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = "MyGroup"

            };
            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(new MsgPackDeserializer<TValue>())
                .SetKeyDeserializer(new MsgPackDeserializer<TKey>())
                .Build();

            _consumer.Subscribe("Topic");
            _data = new List<TValue>();
        }

        public Task Consume()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var cr = _consumer.Consume();

                    _data.Add(cr.Message.Value);
                }
            });

            return Task.CompletedTask;
        }
    }
}

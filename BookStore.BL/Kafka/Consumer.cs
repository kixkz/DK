using BookStore.Models.Configurations;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BookStore.BL.Kafka
{
    public class Consumer<TKey, TValue>
    {
        private readonly IOptionsMonitor<MyKafkaConsumerSettings> _kafkaSettings;
        private readonly IConsumer<TKey, TValue> _consumer;

        public Consumer(IOptionsMonitor<MyKafkaConsumerSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;

            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.CurrentValue.BootstrapServers,
                GroupId = "MyGroup"

            };
            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(new MsgPackDeserializer<TValue>())
                .SetKeyDeserializer(new MsgPackDeserializer<TKey>())
                .Build();

            _consumer.Subscribe("PartTopic");
        }

        public Task Consume()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var cr = _consumer.Consume();

                    KafkaDataList<TValue>.persons.Add(cr.Value);
                }
            });
            
            return Task.CompletedTask;
        }
    }
}

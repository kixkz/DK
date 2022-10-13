using BookStore.Models.Configurations;
using Confluent.Kafka;
using KafkaBasicProducer;
using Microsoft.Extensions.Options;

namespace BookStore.BL.Kafka
{
    public class Producer<Tkey, Tvalue>
    {
        private readonly IOptionsMonitor<MyKafkaProducerSettings> _kafkaSettings;
        private readonly IProducer<Tkey, Tvalue> _producer;

        public Producer(IOptionsMonitor<MyKafkaProducerSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;

            var config = new ProducerConfig()
            {
                BootstrapServers = _kafkaSettings.CurrentValue.BootstrapServers
            };
            

            _producer = new ProducerBuilder<Tkey, Tvalue>(config)
                .SetValueSerializer(new MsgPackSerializer<Tvalue>())
                .SetKeySerializer(new MsgPackSerializer<Tkey>())
                .Build();
        }

        public async Task SendMessage(Tkey tkey, Tvalue tvalue)
        {
            var msg = new Message<Tkey, Tvalue>()
            {
                Key = tkey,
                Value = tvalue
            };

            await _producer.ProduceAsync("PartTopic", msg);
        }
    }
}

namespace BookStore.Models.Configurations
{
    public class MyKafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }
    }
}

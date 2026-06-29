using Microsoft.Extensions.Configuration;
using Confluent.Kafka;

namespace Services
{
    public sealed class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"]
                ?? throw new InvalidOperationException("Kafka:BootstrapServers is missing.");
            _topic = configuration["Kafka:Topic"]
                ?? throw new InvalidOperationException("Kafka:Topic is missing.");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task SendMessageAsync(string message)
        {
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
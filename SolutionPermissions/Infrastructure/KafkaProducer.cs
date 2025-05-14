using Confluent.Kafka;
using Domain.Interfaces;
using System.Text.Json;


namespace Infrastructure
{
    public class KafkaProducer<T> : IKafkaProducer<T>
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducer(string topic,string bootstrapServer)
        {
            _topic = topic;

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServer
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync(T mensaje)
        {
            var json = JsonSerializer.Serialize(mensaje);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
        }
    }
}

using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure
{
    public class KafkaProducer : IProducer
    {
        private KafkaSettings _settings;
        private string _topic;
        private ProducerConfig _producerConfig;
        private SchemaRegistryConfig _schemaRegistryConfig;
        private CachedSchemaRegistryClient _schemaRegistryClient;
        private JsonSerializerConfig _jsonSerializerConfig;
        protected IClient producer;

        public KafkaProducer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        public void Connect<TT>(string topic)
            where TT : class, IMessage
        {
            _topic = topic;
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServer
            };

            _schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _settings.SchemaRegistryUrl
            };

            _schemaRegistryClient = new CachedSchemaRegistryClient(_schemaRegistryConfig);

            _jsonSerializerConfig = new JsonSerializerConfig
            {
                BufferBytes = 100
            };

            producer = new ProducerBuilder<string, TT>(_producerConfig)
               .SetValueSerializer(new JsonSerializer<TT>(_schemaRegistryClient, _jsonSerializerConfig))
               .Build();
        }

        public async Task PublishAsync<TT>(TT message)
             where TT : class, IMessage
        {
            await ((IProducer<string, TT>)producer).ProduceAsync(_topic, new Message<string, TT> { Value = message });
        }

        public void Close()
        {
            producer.Dispose();
            _schemaRegistryClient.Dispose();
        }
    }
}

using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace Common.Infrastructure
{
    public abstract class ProducerBase<T>
        where T : class
    {
        protected readonly string _topic;
        protected readonly ProducerConfig _producerConfig;
        protected readonly SchemaRegistryConfig _schemaRegistryConfig;
        protected IProducer<string, T> producer;
        protected CachedSchemaRegistryClient schemaRegistryClient;

        protected ProducerBase() { }

        public ProducerBase(string bootstrapServers, string schemaRegistryUrl, string topic)
        {
            _topic = topic;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            };
        }

        protected void AddSchemaRegistry()
        {
            schemaRegistryClient = new CachedSchemaRegistryClient(_schemaRegistryConfig);
        }

        public async Task ProduceAsync(T message)
        {
            await producer.ProduceAsync(_topic, new Message<string, T> { Value = message });
        }

        public void Close()
        {
            producer.Dispose();
            schemaRegistryClient.Dispose();
        }
    }
}

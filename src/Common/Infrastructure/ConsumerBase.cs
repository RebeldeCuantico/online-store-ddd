using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace Common.Infrastructure
{
    public abstract class ConsumerBase<T>
        where T : class
    {
        protected string _topic;
        protected CachedSchemaRegistryClient schemaRegistryClient;
        protected readonly ConsumerConfig _consumerConfig;
        protected readonly SchemaRegistryConfig _schemaRegistryConfig;
        protected IConsumer<string, T> consumer;

        protected ConsumerBase() { }

        public ConsumerBase(string bootstrapServer, string schemaRegistryUrl, string consumerGroup, string topic)
        {
            _topic = topic;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                GroupId = consumerGroup
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

        public T Consume()
        {
            try
            {
                var cr = consumer.Consume();
                return cr.Message.Value;
            }

            catch (OperationCanceledException)
            {
                Close();
            }

            return default(T);
        }

        public void Close()
        {
            consumer.Dispose();
            schemaRegistryClient.Dispose();
        }
    }
}

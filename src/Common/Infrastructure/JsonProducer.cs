using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;

namespace Common.Infrastructure
{
    public class JsonProducer<T> : ProducerBase<T>
        where T : class
    {
        private readonly JsonSerializerConfig _jsonSerializerConfig;

        private JsonProducer()
        {            
        }

        public JsonProducer(string bootstrapServers, string schemaRegistryUrl, string topic)
            : base(bootstrapServers, schemaRegistryUrl, topic)
        {
            _jsonSerializerConfig = new JsonSerializerConfig
            {
                BufferBytes = 100
            };
        }

        public void Build()
        {
            base.AddSchemaRegistry();

            producer = new ProducerBuilder<string, T>(_producerConfig)
                .SetValueSerializer(new JsonSerializer<T>(schemaRegistryClient, _jsonSerializerConfig))
                .Build();
        }
    }
}

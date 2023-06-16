using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

namespace Common.Infrastructure
{
    public class JsonConsumer<T> : ConsumerBase<T>
        where T : class
    {
        private JsonConsumer()
        {
        }

        public JsonConsumer(string bootstrapServers, string schemaRegistryUrl, string topic, string consumerGroup)
            : base(bootstrapServers, schemaRegistryUrl, consumerGroup, topic)
        {
        }

        public void Build()
        {
            base.AddSchemaRegistry();

            consumer = new ConsumerBuilder<string, T>(_consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(new JsonDeserializer<T>().AsSyncOverAsync())
                .SetErrorHandler((_, e) =>
                    Console.WriteLine($"Error: {e.Reason}")
                )
                .Build();

            consumer.Subscribe(_topic);
        }
    }
}

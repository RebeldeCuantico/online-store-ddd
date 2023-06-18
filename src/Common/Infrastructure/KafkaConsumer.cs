using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure
{
    public class KafkaConsumer : IConsumer        
    {
        private KafkaSettings _settings;
        private string _topic;
        private ConsumerConfig _consumerConfig;
        private SchemaRegistryConfig _schemaRegistryConfig;
        private CachedSchemaRegistryClient _schemaRegistryClient;
        protected IClient consumer;

        public KafkaConsumer(IOptions<KafkaSettings> options)
        {
            _settings = options.Value;
        }

        public void Connect<TT>(string topic, string cg)
            where TT : class, IMessage
        {
            _topic = topic;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServer,
                GroupId = cg
            };

            _schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _settings.SchemaRegistryUrl
            };

            _schemaRegistryClient = new CachedSchemaRegistryClient(_schemaRegistryConfig);

            consumer = new ConsumerBuilder<string, TT>(_consumerConfig)
               .SetKeyDeserializer(Deserializers.Utf8)
               .SetValueDeserializer(new JsonDeserializer<TT>().AsSyncOverAsync())
               .SetErrorHandler((_, e) =>
                   Console.WriteLine($"Error: {e.Reason}")
               )
               .Build();

            ((IConsumer<string, TT>)consumer).Subscribe(_topic);
        }

        public void Consume<TT>(Action<TT> method)
             where TT : class, IMessage
        {
            try
            {
                var cr = ((IConsumer<string, TT>)consumer).Consume();
                method(cr.Message.Value);
            }

            catch (OperationCanceledException)
            {
                Close();
            }

        }

        public void Close()
        {
            consumer.Dispose();
            _schemaRegistryClient.Dispose();
        }
    }
}

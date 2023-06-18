namespace Common.Infrastructure
{
    public class KafkaServiceBus : IServiceBus
    {
        private readonly IConsumer _kafkaConsumer;
        private readonly IProducer _kafkaProducer;
        private bool _isProducer;
        private bool _isConsumer;

        public KafkaServiceBus(IConsumer kafkaConsumer, IProducer kafkaProducer)
        {
            _kafkaConsumer = kafkaConsumer;
            _kafkaProducer = kafkaProducer;
        }

        public void AsProducer<T>(string topic)
            where T : class, IMessage
        {
            if (_isConsumer) { throw new Exception("The ServiceBus must have only one role"); }
            _kafkaProducer.Connect<T>(topic);
            _isProducer = true;
        }

        public void AsConsumer<T>(string topic, string cg)
            where T : class, IMessage
        {
            if (_isProducer) { throw new Exception("The ServiceBus must have only one role"); }
            _kafkaConsumer.Connect<T>(topic, cg);
            _isConsumer = true;
        }

        public async Task PublishAsync<T>(T message)
            where T : class, IMessage
        {
            await _kafkaProducer.PublishAsync(message);
        }

        public void Consume<T>(Action<T> method)
            where T : class, IMessage
        {
            _kafkaConsumer.Consume<T>(method);
        }
    }
}

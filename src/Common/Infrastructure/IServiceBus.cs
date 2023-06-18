namespace Common.Infrastructure
{
    public interface IServiceBus
    {
        void AsConsumer<T>(string topic, string cg) where T : class, IMessage;
        void AsProducer<T>(string topic) where T : class, IMessage;
        void Consume<T>(Action<T> method) where T : class, IMessage;
        Task PublishAsync<T>(T message) where T : class, IMessage;
    }
}
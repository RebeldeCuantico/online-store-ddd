namespace Common.Infrastructure
{
    public interface IProducer
    {
        void Close();
        void Connect<TT>(string topic) where TT : class, IMessage;
        Task PublishAsync<TT>(TT message) where TT : class, IMessage;
    }
}
namespace Common.Infrastructure
{
    public interface IConsumer
    {
        void Close();
        void Connect<TT>(string topic, string cg) where TT : class, IMessage;
        void Consume<TT>(Action<TT> method) where TT : class, IMessage;
    }
}
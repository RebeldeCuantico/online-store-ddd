namespace Common.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        public Task CommitAsync();
    }
}

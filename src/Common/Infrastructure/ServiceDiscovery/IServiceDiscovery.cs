namespace Common.Infrastructure.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<string> GetServiceAddress(string name);
    }
}

using Consul;

namespace Common.Infrastructure.ServiceDiscovery
{
    public class ConsulServiceDiscovery : IServiceDiscovery
    {
        private readonly IConsulClient _client;

        private ConsulServiceDiscovery()
        {
        }

        public ConsulServiceDiscovery(IConsulClient client)
        {
            _client = client;
        }

        public async Task<string> GetServiceAddress(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var services = await _client.Catalog.Service(name);
            var service = services.Response.FirstOrDefault();
            if (service == null)
            {
                throw new ArgumentException("The name does not exist in Consul");
            }

            return $"http://{service.ServiceAddress}:{service.ServicePort}";
        }
    }
}

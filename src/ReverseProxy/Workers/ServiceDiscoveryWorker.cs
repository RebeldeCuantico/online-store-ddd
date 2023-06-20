using Common.Infrastructure.ServiceDiscovery;

namespace ReverseProxy.Workers
{
    public class ServiceDiscoveryWorker : BackgroundService
    {
        private readonly IServiceDiscovery _serviceDiscovery;

        public ServiceDiscoveryWorker(IServiceDiscovery serviceDiscovery)
        {
            _serviceDiscovery = serviceDiscovery;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          
        }                
    }
}

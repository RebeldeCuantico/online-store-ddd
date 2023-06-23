using Common.Infrastructure.ServiceDiscovery;
using Consul;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceDiscoveryExtensions
    {
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services, IOptions<ServiceDiscoverySettings> options)
        {
            var settings = options.Value;

            return services.AddScoped<IConsulClient, ConsulClient>(p => new ConsulClient(config =>
                                                    {
                                                        config.Address = new Uri(settings.Url);

                                                    }))
                           .AddScoped<IServiceDiscovery, ConsulServiceDiscovery>();

        }
    }
}

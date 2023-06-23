using Common.Infrastructure.ServiceDiscovery;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ReverseProxyExtensions
    {
        public static IServiceCollection AddInternalReverseProxy(this IServiceCollection services, IOptions<ServiceDiscoverySettings> options, IServiceDiscovery serviceDiscovery)
        {
            var serviceDiscoverySettings = options.Value;
            services.AddReverseProxy()
                   .LoadFromMemory(GetRoutes(serviceDiscoverySettings.Apis), GetClusters(serviceDiscovery, serviceDiscoverySettings.Apis));

            return services;
        }

        static IReadOnlyList<RouteConfig> GetRoutes(string[] apis)
        {
            var routeConfigs = new List<RouteConfig>();

            foreach (var api in apis)
            {
                routeConfigs.Add(new RouteConfig
                {
                    RouteId = "route" + Random.Shared.Next(),
                    ClusterId = api,
                    Match = new RouteMatch
                    {
                        Path = $"api/{api}/" + "{**catch-all}"
                    },
                    Transforms = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "PathPattern", "{**catch-all}" }
                            }
                        }

                });
            }

            return routeConfigs;
        }


        static IReadOnlyList<ClusterConfig> GetClusters(IServiceDiscovery serviceDiscovery, string[] apis)
        {
            var clusterConfigs = new List<ClusterConfig>();

            foreach (var api in apis)
            {
                clusterConfigs.Add(new ClusterConfig
                {
                    ClusterId = api,
                    SessionAffinity = new SessionAffinityConfig
                    {
                        Enabled = true,
                        Policy = "Cookie",
                        AffinityKeyName = ".Yarp.ReverseProxy.Affinity"
                    },
                    Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                {
                    { api, new DestinationConfig()
                        {
                          Address = serviceDiscovery.GetServiceAddress(api).Result
                        }
                    }
                }
                });

            }

            return clusterConfigs;

        }
    }

}

using Common.Infrastructure.ServiceDiscovery;
using Consul;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IServiceDiscovery, ConsulServiceDiscovery>();
builder.Services.AddScoped<IConsulClient, ConsulClient>();

builder.Services.AddReverseProxy()
    .LoadFromMemory(GetRoutes(), await GetClusters(builder.Services.BuildServiceProvider().GetRequiredService<IServiceDiscovery>()));

var app = builder.Build();
app.MapReverseProxy();

app.Run();

IReadOnlyList<RouteConfig> GetRoutes()
{
    return new[]
    {
        new RouteConfig
        {
            RouteId = "route" + Random.Shared.Next(),
            ClusterId = "catalog",
            Match = new RouteMatch
            {
                Path = "api/catalog/{**catch-all}"
            },
            Transforms = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "PathPattern", "{**catch-all}" }
                }
            }
        }
    };
}

async Task<IReadOnlyList<ClusterConfig>> GetClusters(IServiceDiscovery serviceDiscovery)
{
    return new[]
    {
        new ClusterConfig
        {
            ClusterId ="catalog",
            SessionAffinity = new SessionAffinityConfig
            {
                Enabled = true,
                Policy = "Cookie",
                AffinityKeyName = ".Yarp.ReverseProxy.Affinity"
            },
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "catalog", new DestinationConfig()
                    {
                        Address = await serviceDiscovery.GetServiceAddress("catalog")
                    }
                }
            }
        }
    };
}




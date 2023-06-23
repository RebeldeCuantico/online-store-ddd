using Common.Infrastructure.ServiceDiscovery;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceDiscoverySettings>(builder.Configuration.GetSection(nameof(ServiceDiscoverySettings)));
var serviceDiscoveryOptions = builder.Services.BuildServiceProvider().GetService<IOptions<ServiceDiscoverySettings>>();
builder.Services.AddServiceDiscovery(serviceDiscoveryOptions);
builder.Services.AddInternalReverseProxy(serviceDiscoveryOptions, builder.Services.BuildServiceProvider().GetRequiredService<IServiceDiscovery>());

var app = builder.Build();
app.MapReverseProxy();

app.Run();





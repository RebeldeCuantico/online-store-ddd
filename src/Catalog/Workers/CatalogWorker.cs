using Catalog.Application.DTOs;
using Catalog.Infrastructure.Settings;
using Common.Infrastructure;
using Microsoft.Extensions.Options;

namespace Catalog.Workers
{
    public class CatalogWorker : BackgroundService
    {
        private JsonConsumer<CategoryDto> consumer;

        public CatalogWorker(IOptions<KafkaSettings> kafkaSettingOptions)
        {
            consumer = new JsonConsumer<CategoryDto>(kafkaSettingOptions.Value.BootstrapServer,
                                                     kafkaSettingOptions.Value.SchemaRegistryUrl,
                                                     "Category",
                                                     "cg-1");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.Build();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = consumer.Consume();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}

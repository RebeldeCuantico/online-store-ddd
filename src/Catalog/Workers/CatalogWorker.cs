using Catalog.Application.DTOs;
using Common.Infrastructure;

namespace Catalog.Workers
{
    public class CatalogWorker : BackgroundService
    {
        private readonly IConsumer _consumer;

        public CatalogWorker(IConsumer consumer)
        {
            _consumer = consumer;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Connect<CategoryDto>("Category", "cg-1");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                //TODO: crear el tópico si no existe
                //_consumer.Consume<CategoryDto>(message =>
                //{
                //    ; //TODO more stuff
                //});
                
            }, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}

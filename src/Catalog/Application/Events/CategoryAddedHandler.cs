using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Catalog.Infrastructure.Settings;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Catalog.Application.Events
{
    public class CategoryAddedHandler
    {
        private readonly IDistributedCache _cache;

        public JsonProducer<CategoryDto> producer { get; private set; }

        public CategoryAddedHandler(IDistributedCache cache, IOptions<KafkaSettings> kafkaSettingOptions)
        {
            _cache = cache;
            producer = new JsonProducer<CategoryDto>(kafkaSettingOptions.Value.BootstrapServer, kafkaSettingOptions.Value.SchemaRegistryUrl, "Category");
            producer.Build();
        }

        public async Task Handle(CategoryAdded categoryAdded)
        {
            var category = new CategoryDto
            {
                Id = categoryAdded.CategoryId,
                Description = categoryAdded.CategoryDescription,
                Name = categoryAdded.CategoryName
            };

            var cache = _cache.SetRecord<CategoryDto>(categoryAdded.CategoryId.ToString(), category);

            var messageBroker = producer.ProduceAsync(category);

            Task.WaitAll(cache, messageBroker);
        }
    }
}

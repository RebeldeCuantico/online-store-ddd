using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.Events
{
    public class CategoryAddedHandler
    {
        private readonly IDistributedCache _cache;
        private readonly IProducer _producer;

        public CategoryAddedHandler(IDistributedCache cache, IProducer producer)
        {
            _cache = cache;
            _producer = producer;
            _producer.Connect<CategoryDto>("Category");
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

            var messageBroker = _producer.PublishAsync(category);

            Task.WaitAll(cache, messageBroker);
        }
    }
}

using Catalog.Domain.DomainEvents;
using Microsoft.Extensions.Caching.Distributed;
using Common.Infrastructure;
using Catalog.Application.DTOs;

namespace Catalog.Application.Events
{
    public class CategoryRemovedHandler
    {
        private readonly IDistributedCache _cache;

        public CategoryRemovedHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Handle(CategoryRemoved categoryRemoved)
        {
            await _cache.RemoveRecord<CategoryDto>(categoryRemoved.Id.ToString());
        }
    }
}

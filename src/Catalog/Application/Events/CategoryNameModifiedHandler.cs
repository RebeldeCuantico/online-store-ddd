using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.Events
{
    public class CategoryNameModifiedHandler
    {
        private readonly IDistributedCache _cache;

        public CategoryNameModifiedHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Handle(CategoryNameModified categoryNameModified)
        {
            var categoryInCache = await _cache.GetRecord<CategoryDto>(categoryNameModified.CategoryId.ToString());
            if (categoryInCache != null)
            {
                categoryInCache.Name = categoryNameModified.NewName;

                await _cache.UpdateRecord(categoryInCache.Id.ToString(), categoryInCache);
            }
        }
    }
}

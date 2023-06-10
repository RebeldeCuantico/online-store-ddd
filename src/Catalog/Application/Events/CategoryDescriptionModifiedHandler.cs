using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Microsoft.Extensions.Caching.Distributed;
using Common.Infrastructure;

namespace Catalog.Application.Events
{
    public class CategoryDescriptionModifiedHandler
    {
        private IDistributedCache _cache;

        public CategoryDescriptionModifiedHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Handle(CategoryDescriptionModified categoryDescriptionModified)
        {
            var categoryInCache = await _cache.GetRecord<CategoryDto>(categoryDescriptionModified.CategoryId.ToString());
            if (categoryInCache != null)
            {
                categoryInCache.Description = categoryDescriptionModified.NewDescription;

                await _cache.UpdateRecord(categoryInCache.Id.ToString(), categoryInCache);
            }
        }
    }
}

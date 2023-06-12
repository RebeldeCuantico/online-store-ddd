using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Microsoft.Extensions.Caching.Distributed;
using Common.Infrastructure;

namespace Catalog.Application.Events
{
    public class CategoryModifiedHandler
    {
        private readonly IDistributedCache _cache;

        public CategoryModifiedHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Handle(CategoryModified categoryModified)
        {    
                var categoryInCache = await _cache.GetRecord<CategoryDto>(categoryModified.CategoryId.ToString());

                if (categoryInCache != null)
                {
                    if (categoryModified.FieldName == CategoryFieldName.CategoryName)
                    {
                        categoryInCache.Name = categoryModified.NewField;
                    }

                    if (categoryModified.FieldName == CategoryFieldName.CategoryDescription)
                    {
                        categoryInCache.Description = categoryModified.NewField;
                    }

                    await _cache.UpdateRecord(categoryInCache.Id.ToString(), categoryInCache);
                }            
        }
    }
}

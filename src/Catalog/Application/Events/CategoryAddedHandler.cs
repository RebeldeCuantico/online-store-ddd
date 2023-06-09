using Catalog.Application.DTOs;
using Catalog.Domain.DomainEvents;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.Events
{
    public class CategoryAddedHandler
    {
        private readonly IDistributedCache _cache;

        public CategoryAddedHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Handle(CategoryAdded categoryAdded)
        {
            await _cache.SetRecord<CategoryDto>(categoryAdded.CategoryId.ToString(),
                                                new CategoryDto
                                                {
                                                    Id = categoryAdded.CategoryId,
                                                    Description = categoryAdded.CategoryDescription,
                                                    Name = categoryAdded.CategoryName
                                                });            
        }
    }
}

using Catalog.Application.DTOs;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application
{
    public class GetCategoryByIdQueryHandler
    {
        private readonly IDistributedCache _cache;

        public GetCategoryByIdQueryHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery getCategoryByIdQuery)
        {
            return await _cache.GetRecord<CategoryDto>(getCategoryByIdQuery.id.ToString());
        }
    }
}

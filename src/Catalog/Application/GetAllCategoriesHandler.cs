using Catalog.Application.DTOs;
using Common.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application
{
    public class GetAllCategoriesHandler
    {
        private readonly IDistributedCache _cache;

        public GetAllCategoriesHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery getCategoriesQuery)
        {
            return await _cache.GetAll<CategoryDto>();
        }
    }
}

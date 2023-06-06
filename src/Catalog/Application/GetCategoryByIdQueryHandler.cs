using Catalog.Domain;
using Common.Domain;

namespace Catalog.Application
{
    public class GetCategoryByIdQueryHandler
    {
        private readonly ICatalogRepository _repository;

        public GetCategoryByIdQueryHandler(ICatalogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery getCategoryByIdQuery)
        {
            return await _repository.GetById(new EntityId(getCategoryByIdQuery.id));
        }
    }
}

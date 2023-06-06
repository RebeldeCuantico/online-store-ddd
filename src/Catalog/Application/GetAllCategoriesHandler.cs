using Catalog.Domain;

namespace Catalog.Application
{
    public class GetAllCategoriesHandler
    {
        private readonly ICatalogRepository _repository;

        public GetAllCategoriesHandler(ICatalogRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery getCategoriesQuery)
        {
            return await _repository.GetAll();
        }
    }
}

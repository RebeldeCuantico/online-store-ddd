using Common.Domain;
using Common.Infrastructure;

namespace Catalog.Domain
{
    public interface ICatalogRepository : IRepositoryBase<Category>
    {
        EntityId AddCategory(Category category);
        Task<List<Category>> GetAll();
        Task<Category> GetById(EntityId entityId);

        EntityId Remove(Category category);

        Category Update(Category category);
    }
}

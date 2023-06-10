using Common.Domain;

namespace Catalog.Domain
{
    public interface ICatalogRepository
    {
        EntityId AddCategory(Category category);
        Task<List<Category>> GetAll();
        Task<Category> GetById(EntityId entityId);

        EntityId Remove(Category category);

        Category Update(Category category);
    }
}

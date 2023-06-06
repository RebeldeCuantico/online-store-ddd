using Catalog.Domain;
using Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository
{
    public class CatalogRepository : ICatalogRepository
    {
        protected DbSet<Category> entity;

        public CatalogRepository(IDbContext context)
        {
            entity = context.Set<Category>();
        }

        public EntityId AddCategory(Category category)
        {
            entity.Add(category);
            return category.Id;
        }

        public async Task<List<Category>> GetAll()
        {
            return await entity.ToListAsync();
        }

        public async Task<Category> GetById(EntityId entityId)
        {
            return await entity.FirstOrDefaultAsync(entity => entity.Id == entityId);
        }
    }
}

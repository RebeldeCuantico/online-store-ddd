using Catalog.Domain;
using Common.Domain;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository
{
    public class CatalogRepository : RepositoryBase<Category>, ICatalogRepository
    {
        public CatalogRepository(IDbContext context)
            : base(context) { }

        public EntityId AddCategory(Category category)
        {
            entity.Add(category);
            SetDates(category);

            return category.Id;
        }

        public EntityId Remove(Category category)
        {
            SetDeleteDate(category);
            return category.Id;
        }

        public async Task<List<Category>> GetAll()
        {
            var now = DateTime.UtcNow;


            return await entity.Where(e => EF.Property<DateTime?>(e, "DeleteDate") == null ||
                                           EF.Property<DateTime?>(e, "DeleteDate") >= now).ToListAsync();

        }


        public async Task<Category> GetById(EntityId entityId)
        {
            var now = DateTime.UtcNow;
            return await entity.FirstOrDefaultAsync(e => e.Id == entityId && (
                                          EF.Property<DateTime?>(e, "DeleteDate") == null ||
                                          EF.Property<DateTime?>(e, "DeleteDate") >= now));
        }

        public Category Update(Category category)
        {
            entity.Update(category);
            SetUpdateDate(category, DateTime.UtcNow);

            return category;
        }

        private void SetDates(Category category)
        {
            var now = DateTime.UtcNow;
            entity.Entry(category).Property("CreateDate").CurrentValue = now;
            SetUpdateDate(category, now);
        }

        private void SetDeleteDate(Category category)
        {
            var now = DateTime.UtcNow;
            SetUpdateDate(category, now);
            entity.Entry(category).Property("DeleteDate").CurrentValue = now;
        }

        private void SetUpdateDate(Category category, DateTime updateDate)
        {
            entity.Entry(category).Property("UpdateDate").CurrentValue = updateDate;
        }
    }
}

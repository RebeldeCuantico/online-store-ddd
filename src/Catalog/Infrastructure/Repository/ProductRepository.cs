using Catalog.Domain;
using Common.Domain;
using Common.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbContext context)
            : base(context) { }


        public EntityId AddProduct(Product product)
        {
            entity.Add(product);
            SetDates(product);

            return product.Id;
        }

        public async Task<Product> GetProductById(EntityId productId)
        {
            var now = DateTime.UtcNow;
            return await entity.FirstOrDefaultAsync(e => e.Id == productId && (
                                          EF.Property<DateTime?>(e, "DeleteDate") == null ||
                                          EF.Property<DateTime?>(e, "DeleteDate") >= now));
        }
    }
}

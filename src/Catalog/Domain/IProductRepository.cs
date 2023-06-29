using Common.Domain;
using Common.Infrastructure;

namespace Catalog.Domain
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public Task<Product> GetProductById(EntityId productId);

        EntityId AddProduct(Product product);
    }
}

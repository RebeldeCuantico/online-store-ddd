using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class ProductAdded : IDomainEvent
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int AvailableStock { get; set; }

        public Guid CategoryId { get; set; }

        public string ProductCode { get; set; }
        
        public string Currency { get; private set; }

        public static explicit operator ProductAdded(Product product)
        {
            return new ProductAdded
            {
                Id = product.Id.Value,
                AvailableStock = product.AvailableStock.Value,
                CategoryId = product.CategoryId.Value,
                ProductCode = product.ProductCode.Value,
                Price = product.Price.Amount,
                Currency = product.Price.Currency.Symbol,
                Name = product.Name.Value,
                Description = product.Description.Value,
            };
        }
    }
}

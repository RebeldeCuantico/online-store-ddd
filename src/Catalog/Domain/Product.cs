using Catalog.Domain.DomainEvents;
using Common.Domain;

namespace Catalog.Domain
{
    public class Product : AggregateRoot
    {
        private Product()
        {
        }

        public Product(EntityId id, Name name, ProductCode productCode, Description description, Price price, AvailableStock availableStock, ReferenceId categoryId)
            : base(id)
        {
            Name = name;
            ProductCode = productCode;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            CategoryId = categoryId;
            QueueEvent((ProductAdded)this);
        }

        public Name Name { get; private set; }

        public Description Description { get; private set; }

        public Price Price { get; private set; }

        public AvailableStock AvailableStock { get; private set; }

        public ReferenceId CategoryId { get; private set; }

        public ProductCode ProductCode { get; private set; }

        public void ChangePrice(Price price)
        {
            var @event = new PriceChanged(Id.Value, Price.Value, price.Value);
            Price = price;
            QueueEvent(@event);
        }

        public void RemoveStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new Exception($"The quantity cannot be {quantity}");
            }

            if (AvailableStock.Value == 0)
            {
                throw new Exception($"The available stock is {AvailableStock.Value}");
            }

            if (AvailableStock.Value < quantity)
            {
                //TODO : generate domain exceptions
                throw new Exception($"The quantity cannot be more than the available stock {AvailableStock.Value}");
            }

            var @event = new StockChanged(Id.Value, AvailableStock.Value, AvailableStock.Value - quantity);
            AvailableStock = new AvailableStock(AvailableStock.Value - quantity);
            QueueEvent(@event);
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new Exception($"The quantity cannot be {quantity}");
            }

            var @event = new StockChanged(Id.Value, AvailableStock.Value, AvailableStock.Value + quantity);
            AvailableStock = new AvailableStock(AvailableStock.Value + quantity);
            QueueEvent(@event);
        }
    }
}

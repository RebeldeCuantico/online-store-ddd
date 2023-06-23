using Common.Domain;

namespace Catalog.Domain
{
    public class Product : AggregateRoot
    {
        private Product()
        {
        }

        public Product(Name name, ProductCode productCode, Description description, Price price, AvailableStock availableStock, ReferenceId categoryId)
        {
            Name = name;
            ProductCode = productCode;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            CategoryId = categoryId;
            //Eventos
        }

        public Name Name { get; private set; }

        public Description Description { get; private set; }

        public Price Price { get; private set; }

        public AvailableStock AvailableStock { get; private set; }

        public ReferenceId CategoryId { get; private set; }

        public ProductCode ProductCode { get; private set; }

        public void ChangePrice(Price price)
        {
            //Eventos 
            Price = price;
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

            AvailableStock = new AvailableStock(AvailableStock.Value - quantity);
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new Exception($"The quantity cannot be {quantity}");
            }

            AvailableStock = new AvailableStock(AvailableStock.Value + quantity);
        }
    }
}

using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class PriceChanged : IDomainEvent
    {
        public PriceChanged(Guid id, decimal oldPrice, decimal newPrice)
        {
            Id = id;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }

        public Guid Id { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
    }
}

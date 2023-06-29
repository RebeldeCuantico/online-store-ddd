using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class StockChanged : IDomainEvent
    {
        public int OldStock { get; set; }

        public int NewStock { get; set; }

        public Guid Id { get; set; }

        public StockChanged(Guid id, int oldStock, int newStock)
        {            
            OldStock = oldStock;
            NewStock = newStock;
            Id = id;
        }
    }
}

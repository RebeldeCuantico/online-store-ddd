using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class AvailableStock : ValueObject
    {
        private AvailableStock() { }

        public AvailableStock(int stock)
        {
            Guard.NotLessThan(stock, 0, nameof(stock), "The stock cannot be negative");
            Value = stock;
        }

        public int Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

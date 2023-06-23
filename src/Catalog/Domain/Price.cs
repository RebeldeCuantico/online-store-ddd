using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class Price : ValueObject
    {
        private Price() { }

        public Price(decimal price)
        {
            Guard.NotLessThanOrEqualTo(price, 0, nameof(price), "The price cannot be negative or zero"); 
            Value = price;
        }

        public decimal Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

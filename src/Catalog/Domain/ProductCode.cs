using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class ProductCode : ValueObject
    {
        private ProductCode() { }

        public ProductCode(string productCode)
        {
            Guard.NotNullOrEmpty(productCode, nameof(productCode), "The productCode Can't be null or empty");
            Value = productCode;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

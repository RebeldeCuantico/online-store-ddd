using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class Description : ValueObject
    {
        public Description() { }

        public Description(string description)
        {
            Guard.NotNullOrEmpty(description, nameof(description), "The description can't be null or empty");
            Value = description;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}

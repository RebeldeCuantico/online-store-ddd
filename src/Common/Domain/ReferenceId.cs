namespace Common.Domain
{
    public class ReferenceId : ValueObject
    {
        private ReferenceId() { }

        public ReferenceId(Guid referenceId)
        {           
            Value = referenceId;
        }

        public Guid Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

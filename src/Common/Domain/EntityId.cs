namespace Common.Domain
{
    public class EntityId : ValueObject
    {
        private EntityId() { }

        public EntityId(Guid id)
        {
            Value = id;
        }

        public Guid Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

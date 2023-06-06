namespace Common.Domain
{
    public abstract class Entity
    {
        protected Entity() { }

        public Entity(EntityId id)
        {
            Id = id;
        }

        public virtual EntityId Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj is not Entity other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetUnproxiedType(this) != GetUnproxiedType(other))
            {
                return false;
            }

            if (Id.Equals(default) || other.Id.Equals(default))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity first, Entity second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null || second is null)
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(Entity first, Entity second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return (GetUnproxiedType(this).ToString() + Id).GetHashCode();
        }

        internal static Type GetUnproxiedType(object obj)
        {
            Type type = obj.GetType();

            return type.BaseType;
        }

    }
}

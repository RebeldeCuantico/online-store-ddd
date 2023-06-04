using Common.Domain;

namespace Catalog.Domain
{
    public class Category : AggregateRoot
    {
        private Category() : base() { }

        public Category(Name name, Description description, EntityId id) 
            : base(id)
        {
            Name = name;
            Description = description;                
        }

        public Name Name { get; private set; }

        public Description Description { get; private set; }

        public void ChangeName(string name)
        {
            Name = new Name(name);
        }

        public void ChangeDescription(string description)
        {
            Description = new Description(description);
        }
    }
}

using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class CategoryRemoved : IDomainEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }

        private CategoryRemoved()
        {            
        }

        public CategoryRemoved(Guid id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}

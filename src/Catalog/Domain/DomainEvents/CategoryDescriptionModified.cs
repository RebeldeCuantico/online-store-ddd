using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class CategoryDescriptionModified : IDomainEvent
    {
        public Guid CategoryId { get; }
        public string OldDescription { get; }
        public string NewDescription { get; }

        private CategoryDescriptionModified() { }

        public CategoryDescriptionModified(Guid categoryId, string oldDescription, string newDescription) 
        {
            CategoryId = categoryId;
            OldDescription = oldDescription;
            NewDescription = newDescription;
        }
    }
}

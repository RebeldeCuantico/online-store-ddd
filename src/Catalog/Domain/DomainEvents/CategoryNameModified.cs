using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class CategoryNameModified : IDomainEvent
    {
        public Guid CategoryId { get; }
        public string OldName { get; }
        public string NewName { get; }

        private CategoryNameModified() { }

        public CategoryNameModified(Guid categoryId, string oldName, string newName) 
        {
            CategoryId = categoryId;
            OldName = oldName;
            NewName = newName;
        }
    }
}

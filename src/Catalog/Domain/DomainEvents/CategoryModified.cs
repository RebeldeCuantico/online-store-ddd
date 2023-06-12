using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class CategoryModified : IDomainEvent
    {
        public CategoryModified(Guid categoryId, string oldField, string newField, CategoryFieldName fieldName)
        {
            CategoryId = categoryId;
            OldField = oldField;
            NewField = newField;
            FieldName = fieldName;
        }

        public Guid CategoryId { get; }

        public string OldField { get; }
        
        public string NewField { get; }

        public CategoryFieldName FieldName { get; }
    }
}

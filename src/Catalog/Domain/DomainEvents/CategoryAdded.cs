using Common.Domain;

namespace Catalog.Domain.DomainEvents
{
    public class CategoryAdded : IDomainEvent
    {
        private CategoryAdded()
        {            
        }

        public CategoryAdded(Category category) 
        {
            CategoryId = category.Id.Value;
            CategoryName = category.Name.Value;
            CategoryDescription = category.Description.Value;   
        }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }
    }
}

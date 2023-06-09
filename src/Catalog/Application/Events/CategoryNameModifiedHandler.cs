using Catalog.Domain.DomainEvents;

namespace Catalog.Application.Events
{
    public class CategoryNameModifiedHandler
    {
        public async Task Handle(CategoryNameModified categoryNameModified)
        {
            //TODO : Stuff
            await Task.FromResult(categoryNameModified);
        }
    }
}
